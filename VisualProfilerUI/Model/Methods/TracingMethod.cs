using System;
using VisualProfilerUI.Model.Criteria;
using VisualProfilerUI.Model.Criteria.TracingCriteria;
using VisualProfilerUI.Model.CriteriaContexts;
using VisualProfilerUI.Model.Values;

namespace VisualProfilerUI.Model.Methods
{
    public class TracingMethod : Method
    {
        private readonly UintValue _callCountValue;
        private readonly Uint64Value _timeWallClockValue;
        private readonly DoubleValue _timeActiveValue;

        public TracingMethod(
            uint id,
            string name,
            int firstLineNumber,
            int lineExtend,
            string classFullName,
            string sourceFile,
            UintValue callCountValue,
            Uint64Value timeWallClockValue,
            DoubleValue timeActiveValue)
            : base(id, name, firstLineNumber, lineExtend,classFullName,sourceFile)
        {
            _callCountValue = callCountValue;
            _timeWallClockValue = timeWallClockValue;
            _timeActiveValue = timeActiveValue;
        }

        public override IValue GetValueFor(Criterion criterion)
        {
            if (criterion == TracingCriteriaContext.CallCountCriterion)
                return _callCountValue;

            if (criterion == TracingCriteriaContext.TimeWallClockCriterion)
                return _timeWallClockValue;

            if (criterion == TracingCriteriaContext.TimeActiveCriterion)
                return _timeActiveValue;

            string message = string.Format("Criterion {0} is not supported for a tracing method", criterion.GetType().Name);
            throw new ArgumentException(message);
        }
    }
}