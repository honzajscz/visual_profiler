using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VisualProfilerUI.Model.Criteria;
using VisualProfilerUI.Model.CriteriaContexts;
using VisualProfilerUI.Model.Values;

namespace VisualProfilerUI.Model.Methods
{
    public class SamplingMethod : Method
    {
        private readonly UintValue _topStackOccurrence;
        private readonly DoubleValue _duration;

        public SamplingMethod(
            uint id,
            string name,
            int firstLineNumber,
            int lineExtend,
            string classFullName,
            string sourceFile,
            UintValue topStackOccurrence,
            DoubleValue duration)
            : base(id, name, firstLineNumber, lineExtend, classFullName, sourceFile)
        {
            _topStackOccurrence = topStackOccurrence;
            _duration = duration;
        }

        public override IValue GetValueFor(Criterion criterion)
        {
            if (criterion == SamplingCriteriaContext.TopStackOccurrenceCriterion)
                return _topStackOccurrence;

            if (criterion == SamplingCriteriaContext.DurationCriterion)
                return _duration;

            string message = string.Format("Criterion {0} is not supported for a tracing method",
                                           criterion.GetType().Name);
            throw new ArgumentException(message);
        }
    }
}
