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

namespace VisualProfilerUI.Model.CallTreeConvertors.Sampling
{

    internal class SamplingTreeElem
    {
        public SamplingTreeElem()
        {
            Children = new List<SamplingTreeElem>();
        }
        public SamplingCallTreeElem CallTreeElem { get; set; }
        public uint InStackSnapshotOccurrence { get; set; }
        public uint TopStackSnapshotOccurrence { get; set; }

        public double WallClockTime { get; set; }
        public List<SamplingTreeElem> Children { get; set; }
    }


    public class SamplingCallTreeConvertor : CallTreeConvertor
    {
        private readonly IEnumerable<SamplingMethodAggregator> _aggregators;
        private readonly SamplingGlobalAggregatedValues _globalAggregatedValues;

        
        private IEnumerable<SamplingTreeElem> RedistributeValues(IEnumerable<SamplingCallTree> tracingCallTrees)
        {
            List<SamplingTreeElem> rootTreeElems = new List<SamplingTreeElem>();
            foreach (var callTree in tracingCallTrees)
            {
                if(callTree.RootElem.Children.Count() ==0)
                    continue;
                SamplingTreeElem recomputedTreeElem;
                RecursiveTreeWalk(callTree.RootElem.Children.First(), out recomputedTreeElem);
                RecursiveTimeTreeWalk(recomputedTreeElem, callTree.WallClockDurationHns, recomputedTreeElem.InStackSnapshotOccurrence);
                rootTreeElems.Add(recomputedTreeElem);
            }
            return rootTreeElems;
        }

        private void RecursiveTimeTreeWalk(SamplingTreeElem treeElem, ulong wallClockDurationHns, uint maxInStackOccurrence)
        {
            treeElem.WallClockTime = wallClockDurationHns*treeElem.InStackSnapshotOccurrence/
                                     (double) maxInStackOccurrence;
            foreach (var childTreeElem in treeElem.Children)
            {
                RecursiveTimeTreeWalk(childTreeElem, wallClockDurationHns, maxInStackOccurrence);
            }
        }

        private void RecursiveTreeWalk(SamplingCallTreeElem callTreeElem, out SamplingTreeElem recomputedTreeElem)
        {
            recomputedTreeElem = new SamplingTreeElem();
            recomputedTreeElem.CallTreeElem = callTreeElem;
            recomputedTreeElem.TopStackSnapshotOccurrence  = //callTreeElem.LastProfiledFrameInStackCount +
                                                               callTreeElem.StackTopOccurrenceCount;
            recomputedTreeElem.InStackSnapshotOccurrence = recomputedTreeElem.TopStackSnapshotOccurrence;
            bool leafReached = callTreeElem.ChildrenCount == 0;
            if(!leafReached)
            {
                foreach (var childCallTreeElem in callTreeElem.Children)
                {
                    SamplingTreeElem childTreeElem;
                    RecursiveTreeWalk(childCallTreeElem, out childTreeElem);
                    recomputedTreeElem.Children.Add(childTreeElem);
                    recomputedTreeElem.InStackSnapshotOccurrence += childTreeElem.InStackSnapshotOccurrence; 
                }
            }
        }

        private void FlattenCallTree(SamplingTreeElem rootElem, List<SamplingTreeElem> flattenedTreeList) 
        {
            flattenedTreeList.AddRange(rootElem.Children);
            foreach (SamplingTreeElem callTreeElem in rootElem.Children)
            {
                FlattenCallTree(callTreeElem, flattenedTreeList);
            }
        }

        public SamplingCallTreeConvertor(IEnumerable<SamplingCallTree> tracingCallTrees)
        {
            var samplingTreeElems = RedistributeValues(tracingCallTrees);

            var flattenedTreeList = new List<SamplingTreeElem>();
            foreach (SamplingTreeElem rootCallTree in samplingTreeElems)
            {
               
                flattenedTreeList.Add(rootCallTree);
                FlattenCallTree(rootCallTree, flattenedTreeList);
            }

            _aggregators = flattenedTreeList.GroupBy(
                elem => elem.CallTreeElem.MethodMetadata).Select(
                    grouping =>
                        {
                            MethodMetadata methodMetadata = grouping.Key;
                            var aggregator = new SamplingMethodAggregator(methodMetadata);
                            aggregator.AggregateRange(grouping);
                            return aggregator;
                        });

            //_globalAggregatedValues.LastProfiledFrameInStackCount = 0;
            //_globalAggregatedValues.StackTopOccurrenceCount = 0;

            //foreach (var methodAgr in _aggregators)
            //{
            //    _globalAggregatedValues.WallClockDurationHns += methodAgr.TimeWallClock;
            //    _globalAggregatedValues.StackTopOccurrenceCount += methodAgr.StackTopOccurrenceCount;
            //}

            CreateMethodByMetadataDictionary();

            // InterconnectMethodCalls(aggregators);


            CreateCriteriaContext();

            PopulateSourceFiles();
        }

        protected void CreateCriteriaContext()
        {
            var maxTopStackOccurrence = new UintValue(uint.MinValue);
            var maxDuration = new DoubleValue(uint.MinValue);
            
            foreach (Method method in _methodDictionary.Values)
            {
                maxTopStackOccurrence =
                    (UintValue) Max(method.GetValueFor(SamplingCriteriaContext.TopStackOccurrenceCriterion), maxTopStackOccurrence);
                maxDuration =
                    (DoubleValue)
                    Max(method.GetValueFor(SamplingCriteriaContext.DurationCriterion), maxDuration);
            }

            CriteriaContext = new SamplingCriteriaContext(
                maxTopStackOccurrence,
                maxDuration);
        }

        protected  void CreateMethodByMetadataDictionary()
        {
            _maxEndLine = 0;
            _methodDictionary = _aggregators.Select(
                methodAgr =>
                    {
                       
                        
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

                        Method method = new SamplingMethod(
                            methodAgr.FunctionId,
                            methodAgr.MethodMd.Name,
                            startLine,
                            endLine - startLine + 1,
                            methodAgr.MethodMd.Class.Name,
                            methodAgr.MethodMd.GetSourceFilePath(),
                            new UintValue(methodAgr.StackTopOccurrenceCount ),
                            new DoubleValue(methodAgr.TimeWallClock));

                        return new KeyValuePair<MethodMetadata, Method>(methodAgr.MethodMd,
                                                                        method);
                    }).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}