using System.IO;
using VisualProfilerAccess.Metadata;

namespace VisualProfilerAccess.ProfilingData.CallTreeElems
{
    public interface ICallTreeElemFactory<TCallTreeElem> where TCallTreeElem : CallTreeElem<TCallTreeElem>
    {
        TCallTreeElem GetCallTreeElem(Stream byteStream, MetadataCache<MethodMetadata> methodCache);
    }

    internal class SamplingCallTreeElemFactory : ICallTreeElemFactory<SamplingCallTreeElem>
    {
        public SamplingCallTreeElem GetCallTreeElem(Stream byteStream, MetadataCache<MethodMetadata> methodCache)
        {
            var samplingCallTreeElem = new SamplingCallTreeElem(byteStream, this, methodCache);
            return samplingCallTreeElem;
        }
    }

    internal class TracingCallTreeElemFactory : ICallTreeElemFactory<TracingCallTreeElem>
    {
        public TracingCallTreeElem GetCallTreeElem(Stream byteStream, MetadataCache<MethodMetadata> methodCache)
        {
            var tracingCallTreeElem = new TracingCallTreeElem(byteStream, this, methodCache);
            return tracingCallTreeElem;
        }
    }
}