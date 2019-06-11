using System;
using System.IO;
using Moq;
using NUnit.Framework;
using VisualProfilerAccess.Metadata;
using VisualProfilerAccess.ProfilingData;
using VisualProfilerAccessTests.MetadataTests;
using VisualProfilerAccessTests.ProfilingDataTests.CallTreeElemsTests;

namespace VisualProfilerAccessTests.ProfilingDataTests.CallTreesTests
{
    [TestFixture]
    public class TestingCallTreeTest
    {
        private readonly byte[] _singleTreeBytes = {0x34, 0x00, 0x00, 0x00, 0x18, 0xE3, 0x4D, 0x00, 0x00};

        private readonly byte[] _twoTreesBytes = {
                                                     0x34, 0x00, 0x00, 0x00, 0x18, 0xE3, 0x49, 0x00, 0x00, 0x00, 0x00,
                                                     0x00, 0x00,
                                                     0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                     0x00, 0x00,
                                                     0x00, 0x00, 0x61, 0x61, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10,
                                                     0x16, 0x26,
                                                     0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x34, 0x34,
                                                     0x2C, 0x00,
                                                     0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x59, 0xAE, 0x97,
                                                     0x00, 0x00,
                                                     0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                     0xC2, 0xC2,
                                                     0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x40,
                                                     0x34, 0x2C,
                                                     0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x2B, 0x79,
                                                     0x8B, 0x00,
                                                     0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                     0x00, 0x84,
                                                     0x85, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
                                                     0x4C, 0x34,
                                                     0x2C, 0x00, 0x14, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x84,
                                                     0xCB, 0x5A,
                                                     0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                     0x00, 0x00,
                                                     0x05, 0x13, 0x58, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                     0x00, 0x58,
                                                     0x34, 0x2C, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00,
                                                     0x48, 0xBE,
                                                     0x28, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                     0x00, 0x00,
                                                     0x00, 0xD2, 0xD8, 0x2A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                     0x00, 0x00,
                                                     0x4C, 0x34, 0x2C, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00,
                                                     0x00, 0xAF,
                                                     0xF7, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                     0x00, 0x00,
                                                     0x00, 0x00, 0xC2, 0xC2, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                     0x00, 0x00,
                                                     0x00, 0x58, 0x34, 0x2C, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00,
                                                     0x00, 0x00,
                                                     0xAF, 0xF7, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                     0x00, 0x00,
                                                     0x00, 0x00, 0x00, 0x61, 0x61, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                     0x00, 0x00,
                                                     0x00, 0x00, 0x34, 0x00, 0x00, 0x00, 0xB8, 0x89, 0x4A, 0x00, 0x00,
                                                     0x00, 0x00,
                                                     0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                     0x00, 0x00,
                                                     0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                     0x00, 0x00,
                                                     0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
                                                 };

        [TestFixtureSetUp]
        public void SetUp()
        {
            _twoTreesStream = _twoTreesBytes.ConvertToMemoryStream();
            _mockMetadataCache = new Mock<MetadataCache<MethodMetadata>>();
            _mockMetadataCache.SetupAllProperties();
            _mockCallTreeElemFactory = new TestingCallTreeElemFactory();
            _callTree1 = new TestingCallTree(_twoTreesStream, _mockCallTreeElemFactory, _mockMetadataCache.Object);
            _callTree2 = new TestingCallTree(_twoTreesStream, _mockCallTreeElemFactory, _mockMetadataCache.Object);
        }


        private const UInt32 ExpectedThreadId = 0x4de318;
        private const ProfilingDataTypes ExpectedProfilingDataType = (ProfilingDataTypes) 0x34;
        private MemoryStream _twoTreesStream;
        private TestingCallTree _callTree1;
        private TestingCallTree _callTree2;
        private TestingCallTreeElemFactory _mockCallTreeElemFactory;
        private Mock<MetadataCache<MethodMetadata>> _mockMetadataCache;

        [Test]
        public void AllBytesReadFromStreamTest()
        {
            Assert.AreEqual(_twoTreesStream.Length, _twoTreesStream.Position);
        }

        [Test]
        public void DeserializationWithoutCallTreeElemsTest()
        {
            var callTree = new TestingCallTree(_singleTreeBytes.ConvertToMemoryStream(), _mockCallTreeElemFactory,
                                               _mockMetadataCache.Object);
            Assert.AreEqual(ExpectedThreadId, callTree.ThreadId);
            Assert.AreEqual(ExpectedProfilingDataType, ProfilingDataTypes.Tracing);
            Assert.AreEqual(ExpectedProfilingDataType, callTree.ProfilingDataType);
        }

        [Test]
        public void TreeElemParentTest()
        {
            TestingCallTreeElem child = _callTree1.RootElem.Children[0].Children[0];
            TestingCallTreeElem parent = _callTree1.RootElem.Children[0];
            Assert.IsTrue(ReferenceEquals(parent, child.ParentElem));
        }

        [Test]
        public void TreeInitializationTest()
        {
            Assert.IsNotNull(_callTree1);
            Assert.IsNotNull(_callTree1.RootElem);
            Assert.AreEqual(2, _callTree1.RootElem.Children[0].Children[0].ChildrenCount);
            Assert.AreEqual(0, _callTree1.RootElem.Children[0].Children[0].Children[0].ChildrenCount);

            Assert.IsNotNull(_callTree2);
            Assert.IsNotNull(_callTree2.RootElem);
            Assert.AreEqual(0, _callTree2.RootElem.ChildrenCount);
        }
    }
}