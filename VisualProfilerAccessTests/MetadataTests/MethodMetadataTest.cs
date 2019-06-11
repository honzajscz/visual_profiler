using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using VisualProfilerAccess.Metadata;
using VisualProfilerAccess.SourceLocation;

namespace VisualProfilerAccessTests.MetadataTests
{
    [TestFixture]
    public class MethodMetadataTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            var mockModuleCache = new Mock<MetadataCache<ModuleMetadata>>(MockBehavior.Strict);
            mockModuleCache.Setup(cache => cache[It.IsAny<uint>()]).Returns(() => null);
            _classMetadata = new ClassMetadata(_classBytes.ConvertToMemoryStream(), mockModuleCache.Object);

            var mockClassCache = new Mock<MetadataCache<ClassMetadata>>(MockBehavior.Strict);
            mockClassCache.Setup(cache => cache[It.IsAny<uint>()]).Returns(_classMetadata);

            _mockMethodLine = new Mock<IMethodLine>(MockBehavior.Strict);
            _mockMethodLine.SetupGet(meLin => meLin.StartLine).Returns(12);
            _mockMethodLine.SetupGet(meLin => meLin.EndLine).Returns(12);
            _mockMethodLine.SetupGet(meLin => meLin.StartIndex).Returns(160);
            _mockMethodLine.SetupGet(meLin => meLin.EndIndex).Returns(220);
            _mockMethodLine.SetupGet(meLin => meLin.StartColumn).Returns(10);
            _mockMethodLine.SetupGet(meLin => meLin.EndColumn).Returns(70);

            var mockSourceLocator = new Mock<ISourceLocator>(MockBehavior.Strict);
            mockSourceLocator.Setup(soLoc => soLoc.GetMethodLines(It.IsAny<uint>())).Returns(
                new[]
                    {
                        _mockMethodLine.Object,
                        _mockMethodLine.Object,
                        _mockMethodLine.Object
                    });

            mockSourceLocator.Setup(soLoc => soLoc.GetSourceFilePath(It.IsAny<uint>())).Returns(SourceFilePath);

            _mockSourceLocatorFactory = new Mock<ISourceLocatorFactory>(MockBehavior.Strict);
            _mockSourceLocatorFactory.Setup(soFac => soFac.GetSourceLocator(It.IsAny<MethodMetadata>())).Returns(
                mockSourceLocator.Object);

            _methodMetadata1 = new MethodMetadata(_method1Bytes.ConvertToMemoryStream(), mockClassCache.Object,
                                                  _mockSourceLocatorFactory.Object);
            _methodMetadata2 = new MethodMetadata(_method2Bytes.ConvertToMemoryStream(), mockClassCache.Object,
                                                  _mockSourceLocatorFactory.Object);
            _methodMetadata3 = new MethodMetadata(_method3Bytes.ConvertToMemoryStream(), mockClassCache.Object,
                                                  _mockSourceLocatorFactory.Object);

            mockClassCache.Verify(cache => cache[It.IsAny<uint>()], Times.Exactly(3));
        }

        private readonly byte[] _classBytes = {
                                                  0x6C, 0x34, 0x21, 0x00, 0x02, 0x00, 0x00, 0x02, 0x2E, 0x00, 0x00, 0x00
                                                  , 0x54, 0x00,
                                                  0x65, 0x00, 0x73, 0x00, 0x74, 0x00, 0x4E, 0x00, 0x61, 0x00, 0x6D, 0x00
                                                  , 0x65, 0x00,
                                                  0x73, 0x00, 0x70, 0x00, 0x61, 0x00, 0x63, 0x00, 0x65, 0x00, 0x2E, 0x00
                                                  , 0x54, 0x00,
                                                  0x65, 0x00, 0x73, 0x00, 0x74, 0x00, 0x43, 0x00, 0x6C, 0x00, 0x61, 0x00
                                                  , 0x73, 0x00,
                                                  0x73, 0x00, 0x00, 0x9C, 0x2E, 0x21, 0x00
                                              };

        private readonly byte[] _method1Bytes = {
                                                    0x34, 0x34, 0x21, 0x00, 0x01, 0x00, 0x00, 0x06, 0x08, 0x00, 0x00,
                                                    0x00, 0x4D,
                                                    0x00, 0x61, 0x00, 0x69, 0x00, 0x6E, 0x00, 0x01, 0x00, 0x00, 0x00,
                                                    0x08, 0x00,
                                                    0x00, 0x00, 0x61, 0x00, 0x72, 0x00, 0x67, 0x00, 0x73, 0x00, 0x6C,
                                                    0x34, 0x21,
                                                    0x00
                                                };

        private readonly byte[] _method2Bytes = {
                                                    0x4C, 0x34, 0x21, 0x00, 0x03, 0x00, 0x00, 0x06,
                                                    0x16, 0x00, 0x00, 0x00, 0x4F, 0x00, 0x74, 0x00, 0x68, 0x00, 0x65,
                                                    0x00, 0x72,
                                                    0x00, 0x4D, 0x00, 0x65, 0x00, 0x74, 0x00, 0x68, 0x00, 0x6F, 0x00,
                                                    0x64, 0x00,
                                                    0x00, 0x00, 0x00, 0x00, 0x6C, 0x34, 0x21, 0x00
                                                };

        private readonly byte[] _method3Bytes = {
                                                    0x58, 0x34, 0x21, 0x00, 0x04, 0x00, 0x00, 0x06, 0x32, 0x00, 0x00,
                                                    0x00, 0x54,
                                                    0x00, 0x65, 0x00, 0x73, 0x00, 0x74, 0x00, 0x4D, 0x00, 0x65, 0x00,
                                                    0x73, 0x00,
                                                    0x73, 0x00, 0x61, 0x00, 0x67, 0x00, 0x65, 0x00, 0x57, 0x00, 0x69,
                                                    0x00, 0x74,
                                                    0x00, 0x68, 0x00, 0x32, 0x00, 0x41, 0x00, 0x72, 0x00, 0x67, 0x00,
                                                    0x75, 0x00,
                                                    0x6D, 0x00, 0x65, 0x00, 0x6E, 0x00, 0x74, 0x00, 0x73, 0x00, 0x02,
                                                    0x00, 0x00,
                                                    0x00, 0x1A, 0x00, 0x00, 0x00, 0x74, 0x00, 0x65, 0x00, 0x73, 0x00,
                                                    0x74, 0x00,
                                                    0x41, 0x00, 0x72, 0x00, 0x67, 0x00, 0x75, 0x00, 0x6D, 0x00, 0x65,
                                                    0x00, 0x6E,
                                                    0x00, 0x74, 0x00, 0x41, 0x00, 0x1A, 0x00, 0x00, 0x00, 0x74, 0x00,
                                                    0x65, 0x00,
                                                    0x73, 0x00, 0x74, 0x00, 0x41, 0x00, 0x72, 0x00, 0x67, 0x00, 0x75,
                                                    0x00, 0x6D,
                                                    0x00, 0x65, 0x00, 0x6E, 0x00, 0x74, 0x00, 0x42, 0x00, 0x6C, 0x34,
                                                    0x21, 0x00
                                                };

        private ClassMetadata _classMetadata;
        private MethodMetadata _methodMetadata1;
        private MethodMetadata _methodMetadata2;
        private MethodMetadata _methodMetadata3;
        private Mock<ISourceLocatorFactory> _mockSourceLocatorFactory;
        private Mock<IMethodLine> _mockMethodLine;
        private const string SourceFilePath = @"c:\code\method1SourceFile.cs";
        private const uint ExpectedId1 = 0x00213434;
        private const uint ExpectedId2 = 0x0021344c;
        private const uint ExpectedId3 = 0x00213458;
        private const uint ExpectedMdToken1 = 0x06000001;
        private const uint ExpectedMdToken2 = 0x06000003;
        private const uint ExpectedMdToken3 = 0x06000004;
        private const string ExpectedName1 = "Main";
        private const string ExpectedName2 = "OtherMethod";
        private const string ExpectedName3 = "TestMessageWith2Arguments";

        [Test]
        public void GetSourceFileTest()
        {
            Assert.AreEqual(SourceFilePath, _methodMetadata1.GetSourceFilePath());
            IEnumerable<IMethodLine> sourceLocations = _methodMetadata1.GetSourceLocations();
            Assert.AreEqual(3, sourceLocations.Count());
            IMethodLine methodLine = sourceLocations.First();
            Assert.AreEqual(12, methodLine.StartLine);
            Assert.AreEqual(12, methodLine.EndLine);
            Assert.AreEqual(160, methodLine.StartIndex);
            Assert.AreEqual(220, methodLine.EndIndex);
            Assert.AreEqual(10, methodLine.StartColumn);
            Assert.AreEqual(70, methodLine.EndColumn);
        }

        [Test]
        public void IdTest()
        {
            Assert.AreEqual(ExpectedId1, _methodMetadata1.Id, "Method1 id does not match.");
            Assert.AreEqual(ExpectedId2, _methodMetadata2.Id, "Method2 id does not match.");
            Assert.AreEqual(ExpectedId3, _methodMetadata3.Id, "Method3 id does not match.");
        }

        [Test]
        public void MdTokenTest()
        {
            Assert.AreEqual(ExpectedMdToken1, _methodMetadata1.MdToken, "Method1 MdToken does not match.");
            Assert.AreEqual(ExpectedMdToken2, _methodMetadata2.MdToken, "Method2 MdToken does not match.");
            Assert.AreEqual(ExpectedMdToken3, _methodMetadata3.MdToken, "Method3 MdToken does not match.");
        }

        [Test]
        public void MetadataTypeTest()
        {
            Assert.AreEqual(MetadataTypes.MethodMedatada, _methodMetadata1.MetadataType);
            Assert.AreEqual(MetadataTypes.MethodMedatada, _methodMetadata2.MetadataType);
            Assert.AreEqual(MetadataTypes.MethodMedatada, _methodMetadata3.MetadataType);
        }

        [Test]
        public void NameTest()
        {
            Assert.AreEqual(ExpectedName1, _methodMetadata1.Name);
            Assert.AreEqual(ExpectedName2, _methodMetadata2.Name);
            Assert.AreEqual(ExpectedName3, _methodMetadata3.Name);
        }

        [Test]
        public void ParemetersTest()
        {
            Assert.AreEqual(1, _methodMetadata1.Parameters.Length);
            Assert.AreEqual("args", _methodMetadata1.Parameters[0]);

            Assert.AreEqual(0, _methodMetadata2.Parameters.Length);

            Assert.AreEqual(2, _methodMetadata3.Parameters.Length);
            Assert.AreEqual("testArgumentA", _methodMetadata3.Parameters[0]);
            Assert.AreEqual("testArgumentB", _methodMetadata3.Parameters[1]);
        }

        [Test]
        public void ParentIdTest()
        {
            Assert.AreEqual(_classMetadata.Id, _methodMetadata1.ClassId);
            Assert.IsTrue(ReferenceEquals(_classMetadata, _methodMetadata1.Class),
                          "Method1's parent module does not match.");

            Assert.AreEqual(_classMetadata.Id, _methodMetadata2.ClassId);
            Assert.IsTrue(ReferenceEquals(_classMetadata, _methodMetadata2.Class),
                          "Method2's parent module does not match.");

            Assert.AreEqual(_classMetadata.Id, _methodMetadata3.ClassId);
            Assert.IsTrue(ReferenceEquals(_classMetadata, _methodMetadata3.Class),
                          "Method3's parent module does not match.");
        }
    }
}