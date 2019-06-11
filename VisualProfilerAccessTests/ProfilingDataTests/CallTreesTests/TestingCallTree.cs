using System.IO;
using VisualProfilerAccess.Metadata;
using VisualProfilerAccess.ProfilingData;
using VisualProfilerAccess.ProfilingData.CallTreeElems;
using VisualProfilerAccess.ProfilingData.CallTrees;
using VisualProfilerAccessTests.ProfilingDataTests.CallTreeElemsTests;

namespace VisualProfilerAccessTests.ProfilingDataTests.CallTreesTests
{
    public class TestingCallTree : CallTree<TestingCallTree, TestingCallTreeElem>
    {
        public TestingCallTree(
            Stream byteStream,
            ICallTreeElemFactory<TestingCallTreeElem> callTreeElemFactory,
            MetadataCache<MethodMetadata> methodCache) : base(byteStream, callTreeElemFactory, methodCache)
        {
        }

        public override ProfilingDataTypes ProfilingDataType
        {
            get { return ProfilingDataTypes.Tracing; }
            // should be ProfilingDataTypes.Mocking but I did not want to spoil the enum with the strange value. 
        }
    }
}