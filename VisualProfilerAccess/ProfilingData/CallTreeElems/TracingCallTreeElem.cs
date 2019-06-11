using System;
using System.IO;
using System.Text;
using VisualProfilerAccess.Metadata;

namespace VisualProfilerAccess.ProfilingData.CallTreeElems
{
    public class TracingCallTreeElem : CallTreeElem<TracingCallTreeElem>
    {
        public TracingCallTreeElem(
            Stream byteStream,
            ICallTreeElemFactory<TracingCallTreeElem> callTreeElemFactory,
            MetadataCache<MethodMetadata> methodCache)
            : base(byteStream, callTreeElemFactory, methodCache)
        {
        }

        public UInt32 EnterCount { get; set; }
        public UInt32 LeaveCount { get; set; }
        public UInt64 WallClockDurationHns { get; set; }
        public UInt64 CycleTime { get; set; }

        protected override void DeserializeFields(Stream byteStream)
        {
            EnterCount = byteStream.DeserializeUint32();
            LeaveCount = byteStream.DeserializeUint32();
            WallClockDurationHns = byteStream.DeserializeUInt64();
            CycleTime = byteStream.DeserializeUInt64();
        }


        protected override void ToString(StringBuilder stringBuilder)
        {
            double durationSec = WallClockDurationHns/1e7;

            double cycleTime = CycleTime/1e6;
            stringBuilder.AppendFormat("{0}, Twc={1}s, CycleTime={2}K, Ec={3}, Lc={4}", MethodMetadata,
                                       durationSec, cycleTime, EnterCount, LeaveCount);
        }
    }
}