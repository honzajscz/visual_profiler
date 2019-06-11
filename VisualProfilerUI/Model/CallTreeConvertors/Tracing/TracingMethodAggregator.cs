using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using VisualProfilerAccess.Metadata;
using VisualProfilerAccess.ProfilingData.CallTreeElems;

namespace VisualProfilerUI.Model.CallTreeConvertors.Tracing
{
    public class TracingMethodAggregator : MethodAggregator<TracingCallTreeElem>
    {
        public UInt32 EnterCount { get; set; }
        public UInt32 LeaveCount { get; set; }
        public UInt64 WallClockDurationHns { get; set; }
        public UInt64 CycleTime { get; set; }

        public TracingMethodAggregator(MethodMetadata methodMd):base(methodMd)
        {}

        protected override void AggregateElemSpecificFields(TracingCallTreeElem callTreeElem)
        {
            EnterCount += callTreeElem.EnterCount;
            LeaveCount += callTreeElem.LeaveCount;
            WallClockDurationHns += callTreeElem.WallClockDurationHns;
            CycleTime += callTreeElem.CycleTime;
        }
    }
}