using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using VisualProfilerUI.Model.Values;

namespace VisualProfilerUITests.ModelTests.ValuesTests
{
    [TestFixture]
    public class DoubleValueTest
    {
        [Test]
        public void CompareEqualsTest()
        {
            DoubleValue value1 = new DoubleValue(12);
            DoubleValue value2 = new DoubleValue(12);
            Assert.AreEqual(0, value1.CompareTo(value2));
        }

        [Test]
        public void CompareLessThanTest()
        {
            DoubleValue value1 = new DoubleValue(12);
            DoubleValue value2 = new DoubleValue(13);
            Assert.AreEqual(-1, value1.CompareTo(value2));
        }

        [Test]
        public void CompareMoreThanTest()
        {
            DoubleValue value1 = new DoubleValue(13);
            DoubleValue value2 = new DoubleValue(12);
            Assert.AreEqual(1, value1.CompareTo(value2));
        }

        [Test]
        public void ConvertToZeroOneScaleTest()
        {
            DoubleValue value1 = new DoubleValue(50);
            DoubleValue value2 = new DoubleValue(100);
            var convertToZeroOneScale = value1.ConvertToZeroOneScale(value2);
            Assert.AreEqual(0.5, convertToZeroOneScale);
        }

        [Test]
        public void ConvertToZeroOneScaleZeroTest()
        {
            DoubleValue value1 = new DoubleValue(50);
            DoubleValue value2 = new DoubleValue(0);
            var convertToZeroOneScale = value1.ConvertToZeroOneScale(value2);
            Assert.AreEqual(0.0, convertToZeroOneScale);
        }

     
    }
}
