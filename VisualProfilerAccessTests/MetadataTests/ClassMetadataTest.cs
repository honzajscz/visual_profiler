using Moq;
using NUnit.Framework;
using VisualProfilerAccess.Metadata;

namespace VisualProfilerAccessTests.MetadataTests
{
    [TestFixture]
    public class ClassMetadataTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            var mockAssemblyCache = new Mock<MetadataCache<AssemblyMetadata>>(MockBehavior.Strict);
            mockAssemblyCache.Setup(cache => cache[It.IsAny<uint>()]).Returns(() => null);
            _moduleMetadata = new ModuleMetadata(_moduleBytes.ConvertToMemoryStream(), mockAssemblyCache.Object);

            _mockModuleCache = new Mock<MetadataCache<ModuleMetadata>>(MockBehavior.Strict);
            _mockModuleCache.Setup(cache => cache[It.IsAny<uint>()]).Returns(() => _moduleMetadata).Verifiable();

            _classMetadata = new ClassMetadata(_classBytes.ConvertToMemoryStream(), _mockModuleCache.Object);

            _mockModuleCache.Verify();
        }

        private readonly byte[] _moduleBytes = {
                                                   0x9C, 0x2E, 0x21, 0x00, 0x01, 0x00, 0x00, 0x06,
                                                   0x80, 0x00, 0x00, 0x00, 0x44, 0x00, 0x3a, 0x00, 0x5c, 0x00, 0x48,
                                                   0x00, 0x6f, 0x00, 0x6e, 0x00, 0x7a, 0x00, 0x69, 0x00, 0x6b, 0x00,
                                                   0x5c, 0x00, 0x44, 0x00, 0x65, 0x00, 0x73, 0x00, 0x6b, 0x00, 0x74,
                                                   0x00, 0x6f, 0x00, 0x70, 0x00, 0x5c, 0x00, 0x4d, 0x00, 0x61, 0x00,
                                                   0x6e, 0x00, 0x64, 0x00, 0x65, 0x00, 0x6c, 0x00, 0x62, 0x00, 0x72,
                                                   0x00, 0x6f, 0x00, 0x74, 0x00, 0x5c, 0x00, 0x4d, 0x00, 0x61, 0x00,
                                                   0x6e, 0x00, 0x64, 0x00, 0x65, 0x00, 0x6c, 0x00, 0x62, 0x00, 0x72,
                                                   0x00, 0x6f, 0x00, 0x74, 0x00, 0x5c, 0x00, 0x62, 0x00, 0x69, 0x00,
                                                   0x6e, 0x00, 0x5c, 0x00, 0x44, 0x00, 0x65, 0x00, 0x62, 0x00, 0x75,
                                                   0x00, 0x67, 0x00, 0x5c, 0x00, 0x4d, 0x00, 0x61, 0x00, 0x6e, 0x00,
                                                   0x64, 0x00, 0x65, 0x00, 0x6c, 0x00, 0x62, 0x00, 0x72, 0x00, 0x6f,
                                                   0x00, 0x74, 0x00, 0x2e, 0x00, 0x65, 0x00, 0x78, 0x00, 0x65, 0x00,
                                                   0x00, 0x3A, 0x37, 0x00
                                               };

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

        //private AssemblyMetadata _assemblyMetadata;
        private ModuleMetadata _moduleMetadata;
        private ClassMetadata _classMetadata;
        private Mock<MetadataCache<ModuleMetadata>> _mockModuleCache;

        private const uint ExpectedId = 0x0021346c;
        private const uint ExpectedMdToken = 0x02000002;
        private const string ExpectedName = "TestNamespace.TestClass";

        [Test]
        public void IdTest()
        {
            Assert.AreEqual(ExpectedId, _classMetadata.Id, "Class id does not match.");
        }

        [Test]
        public void IsGenericTest()
        {
            Assert.IsFalse(_classMetadata.IsGeneric);
        }

        [Test]
        public void MdTokenTest()
        {
            Assert.AreEqual(ExpectedMdToken, _classMetadata.MdToken, "Class MdToken does not match.");
        }

        [Test]
        public void MetadataTypeTest()
        {
            Assert.AreEqual(MetadataTypes.ClassMedatada, _classMetadata.MetadataType);
        }

        [Test]
        public void NameTest()
        {
            Assert.AreEqual(ExpectedName, _classMetadata.Name);
        }

        [Test]
        public void ParentIdTest()
        {
            Assert.AreEqual(_moduleMetadata.Id, _classMetadata.ModuleId);
            Assert.IsTrue(ReferenceEquals(_moduleMetadata, _classMetadata.Module),
                          "Class' parent module does not match.");
        }
    }
}