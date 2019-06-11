using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using VisualProfilerUI.Model.Values;

namespace VisualProfilerUITests.ModelTests.ValuesTests
{
    [TestFixture]
    public class UintValueTest
    {
        [Test]
        public void CompareEqualsTest()
        {
            UintValue uintValue1 = new UintValue(12);
            UintValue uintValue2 = new UintValue(12);
            Assert.AreEqual(0, uintValue1.CompareTo(uintValue2));
        }

        [Test]
        public void CompareLessThanTest()
        {
            UintValue uintValue1 = new UintValue(12);
            UintValue uintValue2 = new UintValue(13);
            Assert.AreEqual(-1, uintValue1.CompareTo(uintValue2));
        }

        [Test]
        public void CompareMoreThanTest()
        {
            UintValue uintValue1 = new UintValue(13);
            UintValue uintValue2 = new UintValue(12);
            Assert.AreEqual(1, uintValue1.CompareTo(uintValue2));
        }

        [Test]
        public void ConvertToZeroOneScaleTest()
        {
            UintValue uintValue1 = new UintValue(50);
            UintValue uintValue2 = new UintValue(100);
            var convertToZeroOneScale = uintValue1.ConvertToZeroOneScale(uintValue2);
            Assert.AreEqual(0.5, convertToZeroOneScale);
        }

        [Test]
        public void ConvertToZeroOneScaleZeroTestTest()
        {
            UintValue uintValue1 = new UintValue(50);
            UintValue uintValue2 = new UintValue(0);
            var convertToZeroOneScale = uintValue1.ConvertToZeroOneScale(uintValue2);
            Assert.AreEqual(0.0, convertToZeroOneScale);
        }
    }
}
