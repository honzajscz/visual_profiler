using System;
using System.IO;
using System.Text;
using VisualProfilerAccess.Metadata;

namespace VisualProfilerAccess.ProfilingData.CallTreeElems
{
    public class SamplingCallTreeElem : CallTreeElem<SamplingCallTreeElem>
    {
        public SamplingCallTreeElem(
            Stream byteStream,
            ICallTreeElemFactory<SamplingCallTreeElem> callTreeElemFactory,
            MetadataCache<MethodMetadata> methodCache)
            : base(byteStream, callTreeElemFactory, methodCache)
        {
        }

        public UInt32 StackTopOccurrenceCount { get; set; }
        public UInt32 LastProfiledFrameInStackCount { get; set; }

        protected override void DeserializeFields(Stream byteStream)
        {
            StackTopOccurrenceCount = byteStream.DeserializeUint32();
            LastProfiledFrameInStackCount = byteStream.DeserializeUint32();
        }

        protected override void ToString(StringBuilder stringBuilder)
        {
            stringBuilder.AppendFormat("{0}, TopFrameCount={1}, LastProfiledFrameCount={2}",
                                       MethodMetadata, StackTopOccurrenceCount,
                                       LastProfiledFrameInStackCount);
        }
    }
}