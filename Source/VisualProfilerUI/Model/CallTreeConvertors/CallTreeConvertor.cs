using System.Collections.Generic;
using System.IO;
using System.Linq;
using VisualProfilerAccess.Metadata;
using VisualProfilerAccess.ProfilingData.CallTreeElems;
using VisualProfilerAccess.SourceLocation;
using VisualProfilerUI.Model.CallTreeConvertors.Tracing;
using VisualProfilerUI.Model.ContainingUnits;
using VisualProfilerUI.Model.CriteriaContexts;
using VisualProfilerUI.Model.Methods;
using VisualProfilerUI.Model.Values;

namespace VisualProfilerUI.Model.CallTreeConvertors
{
    public abstract class CallTreeConvertor
    {
        protected int _maxEndLine;
        protected Dictionary<MethodMetadata, Method> _methodDictionary;
        protected IEnumerable<SourceFile> _sourceFiles;

        public int MaxEndLine
        {
            get { return _maxEndLine; }
        }

        public IEnumerable<SourceFile> SourceFiles
        {
            get { return _sourceFiles; }
        }

        public Dictionary<MethodMetadata, Method> MethodDictionary
        {
            get { return _methodDictionary; }
        }

        public ICriteriaContext CriteriaContext { get; set; }

        private void InterconnectMethodCalls(IEnumerable<TracingMethodAggregator> aggregators)
        {
            foreach (TracingMethodAggregator agr in aggregators)
            {
                Method method = _methodDictionary[agr.MethodMd];
                method.CalledMethods = _methodDictionary.Where(
                    kvp =>
                    agr.CalledFunctions.Contains(kvp.Key)).Select(
                        kvp => kvp.Value).ToArray();
                method.CallingMethods = _methodDictionary.Where(
                    kvp =>
                    agr.CallingFunctions.Contains(kvp.Key)).Select(
                        kvp => kvp.Value).ToArray();
            }
        }

        protected void FindConstructorBody(MethodMetadata method, out int startLine, out int endline)
        {
            IMethodLine openingBrace = method.GetSourceLocations().FirstOrDefault(sl => sl.EndIndex - sl.StartIndex == 1);
            IMethodLine closingBrace = method.GetSourceLocations().LastOrDefault(sl => sl.EndIndex - sl.StartIndex == 1);
            if (openingBrace != null && closingBrace != null)
            {
                startLine = openingBrace.StartLine;
                endline = closingBrace.EndLine;
            }
            else
            {
                startLine = method.GetSourceLocations().First().StartLine;
                endline = method.GetSourceLocations().Last().EndLine;
            }
        }

        protected IValue Max(IValue first, IValue second)
        {
            if (first.CompareTo(second) >= 0)
            {
                return first;
            }
            else
            {
                return second;
            }
        }


        protected void PopulateSourceFiles()
        {
            _sourceFiles = _methodDictionary.GroupBy(kvp => kvp.Key.GetSourceFilePath()).Select(
                kvp =>
                new SourceFile(
                    criteriaContext: CriteriaContext,
                    containedMethods: kvp.Select(k => k.Value).ToArray(),
                    fullName: kvp.Key,
                    displayName: Path.GetFileName(kvp.Key),
                    height: _maxEndLine + 10
                    )).ToArray();
        }

        protected void FlattenCallTree<TCallTreeElem>(TCallTreeElem rootElem, List<TCallTreeElem> flattenedTreeList) where TCallTreeElem : CallTreeElem<TCallTreeElem>
        {
            flattenedTreeList.AddRange(rootElem.Children);
            foreach (TCallTreeElem callTreeElem in rootElem.Children)
            {
                FlattenCallTree(callTreeElem, flattenedTreeList);
            }
        }
    }
}