using Moq;
using NUnit.Framework;
using VisualProfilerAccess.Metadata;

namespace VisualProfilerAccessTests.MetadataTests
{
    [TestFixture]
    public class ModuleMetadataTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            _assemblyMetadata = new AssemblyMetadata(_assemblyBytes.ConvertToMemoryStream());

            _mockAssemblyCache = new Mock<MetadataCache<AssemblyMetadata>>(MockBehavior.Strict);
            _mockAssemblyCache.Setup(cache => cache[It.IsAny<uint>()]).Returns(() => _assemblyMetadata).Verifiable();

            _moduleMetadata = new ModuleMetadata(_moduleBytes.ConvertToMemoryStream(), _mockAssemblyCache.Object);

            _mockAssemblyCache.Verify();
        }

        private readonly byte[] _assemblyBytes = {
                                                     0x00, 0x3A, 0x37, 0x00, 0x01, 0x00, 0x00, 0x20, 0x18, 0x00, 0x00,
                                                     0x00, 0x54, 0x00,
                                                     0x65, 0x00, 0x73, 0x00, 0x74, 0x00, 0x41, 0x00, 0x73, 0x00, 0x73,
                                                     0x00, 0x65, 0x00,
                                                     0x6D, 0x00, 0x62, 0x00, 0x6C, 0x00, 0x79, 0x00, 0x01
                                                 };

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

        private AssemblyMetadata _assemblyMetadata;
        private ModuleMetadata _moduleMetadata;
        private Mock<MetadataCache<AssemblyMetadata>> _mockAssemblyCache;

        private const uint ExpectedId = 0x00212e9c;
        private const uint ExpectedMdToken = 0x06000001;
        private const string ExpectedFile = @"D:\Honzik\Desktop\Mandelbrot\Mandelbrot\bin\Debug\Mandelbrot.exe";

        [Test]
        public void FileTest()
        {
            Assert.AreEqual(ExpectedFile, _moduleMetadata.FilePath);
        }

        [Test]
        public void IdTest()
        {
            Assert.AreEqual(ExpectedId, _moduleMetadata.Id, "Module id does not match.");
        }

        [Test]
        public void MdTokenTest()
        {
            Assert.AreEqual(ExpectedMdToken, _moduleMetadata.MdToken, "Module MdToken does not match.");
        }

        [Test]
        public void MetadataTypeTest()
        {
            Assert.AreEqual(MetadataTypes.ModuleMedatada, _moduleMetadata.MetadataType);
        }

        [Test]
        public void ParentIdTest()
        {
            Assert.IsTrue(ReferenceEquals(_assemblyMetadata, _moduleMetadata.Assembly),
                          "Module's parent assembly does not match.");
        }
    }
}