using System.IO;
using VisualProfilerAccess.Metadata;
using VisualProfilerAccess.ProfilingData.CallTreeElems;

namespace VisualProfilerAccessTests.ProfilingDataTests.CallTreeElemsTests
{
    internal class TestingCallTreeElemFactory : ICallTreeElemFactory<TestingCallTreeElem>
    {
        #region ICallTreeElemFactory<TestingCallTreeElem> Members

        public TestingCallTreeElem GetCallTreeElem(Stream byteStream, MetadataCache<MethodMetadata> methodCache)
        {
            var mockCallTreeElem = new TestingCallTreeElem(byteStream, this, methodCache);
            return mockCallTreeElem;
        }

        #endregion
    }
}