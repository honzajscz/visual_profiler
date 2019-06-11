using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using VisualProfilerAccess.Metadata;
using VisualProfilerAccess.ProfilingData.CallTreeElems;

namespace VisualProfilerUI.Model.CallTreeConvertors
{
    public abstract class MethodAggregator<TCallTreeElem> where TCallTreeElem:CallTreeElem<TCallTreeElem>
    {
        protected MethodAggregator(MethodMetadata methodMd)
        {
            CallingFunctions = new HashSet<MethodMetadata>();
            CalledFunctions = new HashSet<MethodMetadata>();
            FunctionId = methodMd.Id;
            MethodMd = methodMd;
        }

        public uint FunctionId { get; set; }
        public MethodMetadata MethodMd { get; set; }
        public HashSet<MethodMetadata> CallingFunctions { get; set; }
        public HashSet<MethodMetadata> CalledFunctions { get; set; }

        public void Aggregate(TCallTreeElem callTreeElem)
        {
            Contract.Requires(FunctionId == callTreeElem.FunctionId);
            AggregateElemSpecificFields(callTreeElem);
            if (!callTreeElem.ParentElem.IsRootElem())
                CallingFunctions.Add(callTreeElem.ParentElem.MethodMetadata);

            var methodMetadata = callTreeElem.Children.Select(cte => cte.MethodMetadata);
            foreach (var childMethodMetadata in methodMetadata)
            {
                CalledFunctions.Add(childMethodMetadata);
            }
        }

        public void AggregateRange(IEnumerable<TCallTreeElem> callTreeElems)
        {
            foreach (var callTreeElem in callTreeElems)
            {
                Aggregate(callTreeElem);
            }
        }

        protected abstract void AggregateElemSpecificFields(TCallTreeElem callTreeElem);
    }
}