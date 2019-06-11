using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using VisualProfilerAccess.Metadata;

namespace VisualProfilerAccessTests.MetadataTests
{
    [TestFixture]
    public class MetadataBaseTest
    {
        private MetadataBase _metadataBase1;
        private MetadataBase _metadataBase2;
        private MetadataBase _metadataBase3;

        [TestFixtureSetUp]
        public void SetUp()
        {
            Mock<MetadataBase> mockMetadataBase1 = new Mock<MetadataBase>((uint)1,(uint)2);
            mockMetadataBase1.CallBase = true;
           
            Mock<MetadataBase> mockMetadataBase2 = new Mock<MetadataBase>((uint)1, (uint)2);
            mockMetadataBase2.CallBase = true;

            Mock<MetadataBase> mockMetadataBase3 = new Mock<MetadataBase>((uint)4, (uint)2);
            mockMetadataBase2.CallBase = true;

            _metadataBase1 = mockMetadataBase1.Object;
            _metadataBase2 = mockMetadataBase2.Object;
            _metadataBase3 = mockMetadataBase3.Object;
        }

        [Test]
        public void GetHashCodeTest()
        {
            Assert.AreEqual(_metadataBase1.GetHashCode(), _metadataBase2.GetHashCode());
            Assert.AreNotEqual(_metadataBase2.GetHashCode(), _metadataBase3.GetHashCode());
        }

        [Test]
        public void EqualityTest()
        {
            Assert.IsTrue(_metadataBase1.Equals(_metadataBase2));
            Assert.False(_metadataBase1.Equals(_metadataBase3));
            //Assert.AreEqual();
        }
    }
}
