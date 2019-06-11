using System.IO;
using System.Text;
using VisualProfilerAccess.Metadata;
using VisualProfilerAccess.ProfilingData.CallTreeElems;

namespace VisualProfilerAccessTests.ProfilingDataTests
{
    public abstract class StubCallTreeElem : CallTreeElem<StubCallTreeElem>
    {
        public StubCallTreeElem(Stream byteStream, ICallTreeElemFactory<StubCallTreeElem> callTreeElemFactory,
                                MetadataCache<MethodMetadata> methodCache)
            : base(byteStream, callTreeElemFactory, methodCache)
        {
        }

        protected abstract override void DeserializeFields(Stream byteStream);

        protected abstract override void ToString(StringBuilder stringBuilder);
    }
}