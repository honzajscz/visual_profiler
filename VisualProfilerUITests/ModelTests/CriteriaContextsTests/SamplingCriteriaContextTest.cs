using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using VisualProfilerUI.Model.Criteria.SamplingCriteria;
using VisualProfilerUI.Model.CriteriaContexts;
using VisualProfilerUI.Model.Values;

namespace VisualProfilerUITests.ModelTests.CriteriaContextsTests
{
    [TestFixture]
    public class SamplingCriteriaContextTest
    {
        private DoubleValue _maxDuration;
        private UintValue _maxTopStackOccurrence;
        private SamplingCriteriaContext _samplingCriteriaContext;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _maxTopStackOccurrence = new UintValue(50);
            _maxDuration = new DoubleValue(100);
            _samplingCriteriaContext = new SamplingCriteriaContext(_maxTopStackOccurrence, _maxDuration);
            
        }

        [Test]
        public void AvailableCriteriaTest()
        {
            var availableCriteria = _samplingCriteriaContext.AvailableCriteria.ToArray();
            Assert.AreEqual(2,availableCriteria.Count());
            Assert.IsTrue(availableCriteria.Contains(new DurationCriterion()));
            Assert.IsTrue(availableCriteria.Contains(new TopStackOccurrenceCriterion()));
        }

        [Test]
        public void GetMaxValuesTest()
        {
            IValue durationMaxValue = _samplingCriteriaContext.GetMaxValueFor(new DurationCriterion());
            Assert.AreEqual(_maxDuration, durationMaxValue);

            IValue topStackOccurrenceMaxValue = _samplingCriteriaContext.GetMaxValueFor(new TopStackOccurrenceCriterion());
            Assert.AreEqual(_maxTopStackOccurrence, topStackOccurrenceMaxValue);
        }
    }
}
