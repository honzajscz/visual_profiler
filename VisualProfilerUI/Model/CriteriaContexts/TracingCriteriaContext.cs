using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using VisualProfilerUI.Model.Criteria;
using VisualProfilerUI.Model.Criteria.TracingCriteria;
using VisualProfilerUI.Model.Values;

namespace VisualProfilerUI.Model.CriteriaContexts
{
    public class TracingCriteriaContext : ICriteriaContext
    {
        private readonly UintValue _maxCallCount;
        private readonly Uint64Value _maxTimeWallClock;
        private readonly DoubleValue _maxTimeActive;
        private readonly Criterion[] _availableCriteria;
       public static readonly CallCountCriterion CallCountCriterion = new CallCountCriterion();
       public static readonly TimeWallClockCriterion TimeWallClockCriterion = new TimeWallClockCriterion();
       public static readonly TimeActiveCriterion TimeActiveCriterion = new TimeActiveCriterion();

        public TracingCriteriaContext(UintValue maxCallCount, Uint64Value maxTimeWallClock, DoubleValue maxTimeActive)
        {
            Contract.Requires(maxCallCount != null);
            Contract.Requires(maxTimeWallClock != null);
            Contract.Requires(maxTimeActive != null);
            _maxCallCount = maxCallCount;
            _maxTimeWallClock = maxTimeWallClock;
            _maxTimeWallClock = maxTimeWallClock;
            _maxTimeActive = maxTimeActive;
            _availableCriteria = new Criterion[] { CallCountCriterion, TimeWallClockCriterion, TimeActiveCriterion };
        }

        public IEnumerable<Criterion> AvailableCriteria
        {
            get { return _availableCriteria; }
        }

        public IValue GetMaxValueFor(Criterion criterion)
        {
            if (criterion == CallCountCriterion)
                return _maxCallCount;
            if (criterion == TimeWallClockCriterion)
                return _maxTimeWallClock;
            if (criterion == TimeActiveCriterion)
                return _maxTimeActive;

            var exceptionMessage = string.Format("Criterion of type {0} is not available in {1}", criterion.GetType().Name,
                                                 this.GetType().Name);
            throw new ArgumentException(exceptionMessage);

        }
    }
}