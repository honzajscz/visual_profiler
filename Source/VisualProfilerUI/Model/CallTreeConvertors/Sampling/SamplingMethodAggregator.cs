using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using VisualProfilerAccess.Metadata;
using VisualProfilerAccess.ProfilingData.CallTreeElems;

namespace VisualProfilerUI.Model.CallTreeConvertors.Sampling
{
     class SamplingMethodAggregator 
    {
       public uint StackTopOccurrenceCount { get; set; }
       public double TimeWallClock { get; set; }

        public SamplingMethodAggregator(MethodMetadata methodMd)  {
         
            FunctionId = methodMd.Id;
            MethodMd = methodMd;
        }

        protected void AggregateElemSpecificFields(SamplingTreeElem callTreeElem)
        {
            StackTopOccurrenceCount = callTreeElem.TopStackSnapshotOccurrence;
            TimeWallClock = callTreeElem.WallClockTime;
        }

    

        public uint FunctionId { get; set; }
        public MethodMetadata MethodMd { get; set; }


        public void Aggregate(SamplingTreeElem callTreeElem)
        {
            Contract.Requires(FunctionId == callTreeElem.CallTreeElem.FunctionId);
            AggregateElemSpecificFields(callTreeElem);
          
        }

        public void AggregateRange(IEnumerable<SamplingTreeElem> callTreeElems)
        {
            foreach (var callTreeElem in callTreeElems)
            {
                Aggregate(callTreeElem);
            }
        }
    }
}