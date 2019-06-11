using System;
using System.IO;
using System.Text;
using VisualProfilerAccess;
using VisualProfilerAccess.Metadata;
using VisualProfilerAccess.ProfilingData.CallTreeElems;

namespace VisualProfilerAccessTests.ProfilingDataTests.CallTreeElemsTests
{
    public class TestingCallTreeElem : CallTreeElem<TestingCallTreeElem>
    {
        public TestingCallTreeElem(Stream byteStream, ICallTreeElemFactory<TestingCallTreeElem> callTreeElemFactory,
                                   MetadataCache<MethodMetadata> methodCache)
            : base(byteStream, callTreeElemFactory, methodCache)
        {
        }

        public UInt32 EnterCount { get; set; }
        public UInt32 LeaveCount { get; set; }
        public UInt64 WallClockDurationHns { get; set; }
        public UInt64 KernelModeDurationHns { get; set; }
        public UInt64 UserModeDurationHns { get; set; }

        protected override void DeserializeFields(Stream byteStream)
        {
            EnterCount = byteStream.DeserializeUint32();
            LeaveCount = byteStream.DeserializeUint32();
            WallClockDurationHns = byteStream.DeserializeUInt64();
            KernelModeDurationHns = byteStream.DeserializeUInt64();
            UserModeDurationHns = byteStream.DeserializeUInt64();
        }

        protected override void ToString(StringBuilder stringBuilder)
        {
            double durationSec = WallClockDurationHns/1e7;
            double userModeSec = UserModeDurationHns/1e7;
            double kernelModeSec = KernelModeDurationHns/1e7;
            //UInt64 durationSec = WallClockDurationHns; // 1e7;
            //UInt64 userModeSec = UserModeDurationHns ;// 1e7;
            //UInt64 kernelModeSec = KernelModeDurationHns ;// 1e7;
            stringBuilder.AppendFormat("{0},Twc={1}s,Tum={2}s,Tkm={3}s,Ec={4},Lc={5}", MethodMetadata,
                                       durationSec, userModeSec, kernelModeSec, EnterCount, LeaveCount);
        }
    }
}