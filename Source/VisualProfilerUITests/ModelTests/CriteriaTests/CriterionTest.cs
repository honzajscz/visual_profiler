using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using VisualProfilerUI.Model.Criteria;
using VisualProfilerUI.Model.Criteria.SamplingCriteria;

namespace VisualProfilerUITests.ModelTests.CriteriaTests
{
    [TestFixture]
    public class CriterionTest
    {
        [Test]
        public void EqualityTest()
        {
            Criterion criterion1 = new Criterion("this is a random text");
            Criterion criterion2 = new Criterion("this is a random text");
            Assert.IsTrue(criterion1 == criterion2);
            Assert.IsTrue(criterion1.Equals((object)criterion2));
            Assert.IsTrue(criterion1.Equals(criterion2));

        }

        [Test]
        public void EqualityOfDerivedClassTest()
        {
            DurationCriterion criterion1 = new DurationCriterion();
            DurationCriterion criterion2 = new DurationCriterion();
            Assert.IsTrue(criterion1 == criterion2);
            Assert.IsTrue(criterion1.Equals((object)criterion2));
            Assert.IsTrue(criterion1.Equals(criterion2));
        }

        [Test]
        public void InequalityTest()
        {
            Criterion criterion1 = new Criterion("this is a random text");
            Criterion criterion2 = new Criterion("this also");
            Assert.IsTrue(criterion1 != criterion2);
            Assert.IsFalse(criterion1.Equals((object)criterion2));
            Assert.IsFalse(criterion1.Equals(criterion2));
        }
    }
}
