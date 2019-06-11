using System;
using System.IO;
using System.Text;
using VisualProfilerAccess.Metadata;
using VisualProfilerAccess.ProfilingData.CallTreeElems;

namespace VisualProfilerAccess.ProfilingData.CallTrees
{
    public class TracingCallTree : CallTree<TracingCallTree, TracingCallTreeElem>
    {
        public TracingCallTree(Stream byteStream, ICallTreeElemFactory<TracingCallTreeElem> callTreeElemFactory,
                               MetadataCache<MethodMetadata> methodCache)
            : base(byteStream, callTreeElemFactory, methodCache)
        {
        }

        public UInt64 KernelModeDurationHns { get; set; }
        public UInt64 UserModeDurationHns { get; set; }

        public override ProfilingDataTypes ProfilingDataType
        {
            get { return ProfilingDataTypes.Tracing; }
        }

        protected override void DeserializeFields(Stream byteStream)
        {
            KernelModeDurationHns = byteStream.DeserializeUInt64();
            UserModeDurationHns = byteStream.DeserializeUInt64();
        }

        public override void ConvertToString(StringBuilder stringBuilder)
        {
            double userModeSec = UserModeDurationHns/1e7;
            double kernelModeSec = KernelModeDurationHns/1e7;
            stringBuilder.AppendFormat("Tum={0}s,Tkm={1}s", userModeSec, kernelModeSec);
        }

        public UInt64 TotalCycleTime()
        {
            ulong totalCycleTime = TotalCycleTime(RootElem);
            return totalCycleTime;
        }

        public UInt64 TotalUserKernelTime()
        {
            ulong totalUserKernelTime = KernelModeDurationHns + UserModeDurationHns;
            return totalUserKernelTime;
        }

        public static UInt64 TotalCycleTime(TracingCallTreeElem tracingCallTree)
        {
            ulong result = tracingCallTree.IsRootElem() ? 0 : tracingCallTree.CycleTime;

            foreach (TracingCallTreeElem child in tracingCallTree.Children)
                result = result + TotalCycleTime(child);
            return result;
        }
    }
}