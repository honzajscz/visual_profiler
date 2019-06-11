using System.IO;
using VisualProfilerAccess.Metadata;
using VisualProfilerAccess.ProfilingData;
using VisualProfilerAccess.ProfilingData.CallTreeElems;
using VisualProfilerAccess.ProfilingData.CallTrees;

namespace VisualProfilerAccessTests.ProfilingDataTests
{
    public abstract class StubCallTree : CallTree<StubCallTree, StubCallTreeElem>
    {
        public StubCallTree(Stream byteStream, ICallTreeElemFactory<StubCallTreeElem> callTreeElemFactory,
                            MetadataCache<MethodMetadata> methodCache)
            : base(byteStream, callTreeElemFactory, methodCache)
        {
        }

        public abstract override ProfilingDataTypes ProfilingDataType { get; }
    }
}