using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using VisualProfilerUI.Model.ContainingUnits;
using VisualProfilerUI.Model.Criteria;
using VisualProfilerUI.Model.CriteriaContexts;
using VisualProfilerUI.Model.Values;

namespace VisualProfilerUITests
{
    [TestFixture]
    public class ViewModelsTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            Criterion criterion01 = new Criterion("criterion01");
            Criterion criterion02 = new Criterion("criterion02");
            Mock<ICriteriaContext> mock = new Mock<ICriteriaContext>(MockBehavior.Strict);
            mock.SetupGet(cc => cc.AvailableCriteria).Returns(new[]
                                                                  {
                                                                     criterion01,
                                                                     criterion02
                                                                  });
            mock.Setup(cc => cc.GetMaxValueFor(criterion01)).Returns(new UintValue(50));
            mock.Setup(cc => cc.GetMaxValueFor(criterion02)).Returns(new UintValue(60));

            var availableCriteria = mock.Object.AvailableCriteria;
            var maxValueFor = mock.Object.GetMaxValueFor(criterion01);

          //  ContainingUnit containingUnit = new ContainingUnit();
        }

        [Test]
        public void NameTest()
        {
                
            //Assert.AreEqual();
        }
    }
}
