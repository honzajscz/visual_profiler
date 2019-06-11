using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VisualProfilerAccess.Metadata;
using VisualProfilerAccess.ProfilingData.CallTreeElems;
using VisualProfilerAccess.ProfilingData.CallTrees;
using VisualProfilerUI.Model.ContainingUnits;
using VisualProfilerUI.Model.CriteriaContexts;
using VisualProfilerUI.Model.Methods;
using VisualProfilerUI.Model.Values;

namespace VisualProfilerUI.Model.CallTreeConvertors.Tracing
{
    public class TracingCallTreeConvertor : CallTreeConvertor
    {
        private readonly IEnumerable<TracingMethodAggregator> _aggregators;
        private readonly TracingGlobalAggregatedValues _globalAggregatedValues;

        public TracingCallTreeConvertor(IEnumerable<TracingCallTree> tracingCallTrees)
        {
            _globalAggregatedValues = new TracingGlobalAggregatedValues();

            var flattenedTreeList = new List<TracingCallTreeElem>();
            foreach (TracingCallTree callTree in tracingCallTrees)
            {
                _globalAggregatedValues.TotalActiveTime += callTree.UserModeDurationHns + callTree.KernelModeDurationHns;
                FlattenCallTree(callTree.RootElem, flattenedTreeList);
            }

            _aggregators = flattenedTreeList.GroupBy(
                elem => elem.MethodMetadata).Select(
                    grouping =>
                        {
                            MethodMetadata methodMetadata = grouping.Key;
                            var aggregator = new TracingMethodAggregator(methodMetadata);
                            aggregator.AggregateRange(grouping);
                            return aggregator;
                        });


            _globalAggregatedValues.TotalCycleTime = _aggregators.Aggregate((ulong) 0,
                                                                            (sum, methodAgr) =>
                                                                            sum + methodAgr.CycleTime);

            CreateMethodByMetadataDictionary();
            // InterconnectMethodCalls(aggregators);
            CreateCriteriaContext();
            PopulateSourceFiles();
        }

        protected void CreateCriteriaContext()
        {
            var maxCallCount = new UintValue(uint.MinValue);
            var maxWallClockDuration = new Uint64Value(uint.MinValue);
            var maxActiveTime = new DoubleValue(double.MinValue);
            foreach (Method method in _methodDictionary.Values)
            {
                maxCallCount =
                    (UintValue) Max(method.GetValueFor(TracingCriteriaContext.CallCountCriterion), maxCallCount);
                maxWallClockDuration =
                    (Uint64Value)
                    Max(method.GetValueFor(TracingCriteriaContext.TimeWallClockCriterion), maxWallClockDuration);
                maxActiveTime =
                    (DoubleValue) Max(method.GetValueFor(TracingCriteriaContext.TimeActiveCriterion), maxActiveTime);
            }

            CriteriaContext = new TracingCriteriaContext(
                maxCallCount,
                maxWallClockDuration,
                maxActiveTime);
        }

        protected  void CreateMethodByMetadataDictionary()
        {
            _maxEndLine = 0;
            _methodDictionary = _aggregators.Select(
                methodAgr =>
                    {
                        double activeTime = methodAgr.CycleTime*
                                            _globalAggregatedValues.TotalActiveTime/
                                            (double) _globalAggregatedValues.TotalCycleTime;
                        ;
                        int startLine;
                        int endLine;
                        bool isConstructor = methodAgr.MethodMd.Name.EndsWith("ctor");
                        if (isConstructor)
                            FindConstructorBody(methodAgr.MethodMd, out startLine,
                                                out endLine);
                        else
                        {
                            startLine =
                                methodAgr.MethodMd.GetSourceLocations().First().StartLine;
                            endLine = methodAgr.MethodMd.GetSourceLocations().Last().EndLine;
                        }
                        _maxEndLine = Math.Max(_maxEndLine, endLine);

                        Method method = new TracingMethod(
                            methodAgr.FunctionId,
                            methodAgr.MethodMd.Name,
                            startLine,
                            endLine - startLine + 1,
                            methodAgr.MethodMd.Class.Name,
                            methodAgr.MethodMd.GetSourceFilePath(),
                            new UintValue(methodAgr.EnterCount),
                            new Uint64Value(methodAgr.WallClockDurationHns),
                            new DoubleValue(activeTime));

                        return new KeyValuePair<MethodMetadata, Method>(methodAgr.MethodMd,
                                                                        method);
                    }).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

      
    }
}