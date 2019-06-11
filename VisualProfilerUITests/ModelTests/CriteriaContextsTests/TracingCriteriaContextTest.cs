using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using VisualProfilerUI.Model.Criteria.TracingCriteria;
using VisualProfilerUI.Model.CriteriaContexts;
using VisualProfilerUI.Model.Values;

namespace VisualProfilerUITests.ModelTests.CriteriaContextsTests
{
    [TestFixture]
    public class TracingCriteriaContextTest
    {
        private CallCountCriterion _callCountCriterion;
        private TimeActiveCriterion _timeActiveCriterion;
        private TimeWallClockCriterion _timeWallClockCriterion;
        private TracingCriteriaContext _tracingCriteriaContext;
        private DoubleValue _maxTimeActive;
        private Uint64Value _maxTimeWallClock;
        private UintValue _maxCallCount;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _callCountCriterion = new CallCountCriterion();
            _timeActiveCriterion = new TimeActiveCriterion();
            _timeWallClockCriterion = new TimeWallClockCriterion();

            _maxCallCount = new UintValue(50);
            _maxTimeWallClock = new Uint64Value(100);
            _maxTimeActive = new DoubleValue(150);
            _tracingCriteriaContext = new TracingCriteriaContext(_maxCallCount, _maxTimeWallClock, _maxTimeActive);
        }

        [Test]
        public void AvailableCriteriaTest()
        {
            var availableCriteria = _tracingCriteriaContext.AvailableCriteria.ToArray();
            Assert.AreEqual(3, availableCriteria.Count());
            Assert.IsTrue(availableCriteria.Contains(_callCountCriterion));
            Assert.IsTrue(availableCriteria.Contains(_timeActiveCriterion));
            Assert.IsTrue(availableCriteria.Contains(_timeWallClockCriterion));
        }

        [Test]
        public void GetMaxValuesTest()
        {
            IValue callCountMaxValue = _tracingCriteriaContext.GetMaxValueFor(_callCountCriterion);
            Assert.AreEqual(_maxCallCount, callCountMaxValue);

            IValue timeWallClockMaxValue = _tracingCriteriaContext.GetMaxValueFor(_timeWallClockCriterion);
            Assert.AreEqual(_maxTimeWallClock, timeWallClockMaxValue);

            IValue timeActiveMaxValue = _tracingCriteriaContext.GetMaxValueFor(_timeActiveCriterion);
            Assert.AreEqual(_maxTimeActive, timeActiveMaxValue);
        }

    }
}
