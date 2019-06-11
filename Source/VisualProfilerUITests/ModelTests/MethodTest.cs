using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using VisualProfilerUI.Model;
using VisualProfilerUI.Model.ContainingUnits;
using VisualProfilerUI.Model.Criteria;
using VisualProfilerUI.Model.Criteria.SamplingCriteria;
using VisualProfilerUI.Model.Criteria.TracingCriteria;
using VisualProfilerUI.Model.CriteriaContexts;
using VisualProfilerUI.Model.Methods;
using VisualProfilerUI.Model.Values;

namespace VisualProfilerUITests.ModelTests
{
    [TestFixture]
    public class TracingMethodTest
    {
        private Method _method;
        private UintValue _enterCount;
        private Uint64Value _wallClockDurationHns;
        private DoubleValue _activeTime;


        [TestFixtureSetUp]
        public void SetUp()
        {
            _enterCount = new UintValue(1);
            _wallClockDurationHns = new Uint64Value(2);
            _activeTime = new DoubleValue(3);
            Mock<IEnumerable<Method>> mockCallingMethods = new Mock<IEnumerable<Method>>(MockBehavior.Strict);
            Mock<IEnumerable<Method>> mockCalledMethods = new Mock<IEnumerable<Method>>(MockBehavior.Strict);
            _method = new TracingMethod(1,"stub", 20, 50, "MethodFull", @"C:\code\source.cs",_enterCount,_wallClockDurationHns,_activeTime );//, mockCallingMethods.Object,
                                 //mockCalledMethods.Object);
        }

        [Test]
        public void GetValueForTest()
        {
            IValue callCountValue = _method.GetValueFor(TracingCriteriaContext.CallCountCriterion);
            Assert.AreEqual(_enterCount, callCountValue);

            IValue timeWallClock = _method.GetValueFor(TracingCriteriaContext.TimeWallClockCriterion);
            Assert.AreEqual(_wallClockDurationHns, timeWallClock);

            IValue timeActive = _method.GetValueFor(TracingCriteriaContext.TimeActiveCriterion);
            Assert.AreEqual(_activeTime,timeActive);
        }
    }
}
