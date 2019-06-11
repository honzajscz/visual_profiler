using System.IO;
using Moq;
using NUnit.Framework;
using VisualProfilerAccess;
using VisualProfilerAccess.Metadata;
using VisualProfilerAccess.SourceLocation;

namespace VisualProfilerAccessTests.MetadataTests
{
    [TestFixture]
    public class MetadataDeserializerEmptyStreamTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            _methodCache = new MetadataCache<MethodMetadata>();
            _classCache = new MetadataCache<ClassMetadata>();
            _moduleCache = new MetadataCache<ModuleMetadata>();
            _assemblyCache = new MetadataCache<AssemblyMetadata>();
            var mockSourceLocatorFaktory = new Mock<ISourceLocatorFactory>(MockBehavior.Strict);
            _srcLocatorMockupFkt = mockSourceLocatorFaktory.Object;
        }

        private readonly byte[] _empty = {0x00, 0x00, 0x00, 0x00};
        private readonly byte[] _emptyWithOffset = {0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00};
        private MetadataCache<MethodMetadata> _methodCache;
        private MetadataCache<ClassMetadata> _classCache;
        private MetadataCache<ModuleMetadata> _moduleCache;
        private MetadataCache<AssemblyMetadata> _assemblyCache;
        private ISourceLocatorFactory _srcLocatorMockupFkt;

        [Test]
        public void EmptyDataTest()
        {
            var metadataDeserializer = new MetadataDeserializer(
                _methodCache,
                _classCache,
                _moduleCache,
                _assemblyCache,
                _srcLocatorMockupFkt);
            metadataDeserializer.DeserializeAllMetadataAndCacheIt(_empty.ConvertToMemoryStream());

            Assert.AreEqual(0, _methodCache.Cache.Count);
            Assert.AreEqual(0, _classCache.Cache.Count);
            Assert.AreEqual(0, _moduleCache.Cache.Count);
            Assert.AreEqual(0, _assemblyCache.Cache.Count);
        }

        [Test]
        public void EmptyDataWithOffsetInStreamTest()
        {
            MemoryStream memoryStream = _emptyWithOffset.ConvertToMemoryStream();
            uint dummy = memoryStream.DeserializeUint32();

            var metadataDeserializer = new MetadataDeserializer(
                _methodCache,
                _classCache,
                _moduleCache,
                _assemblyCache,
                _srcLocatorMockupFkt);
            metadataDeserializer.DeserializeAllMetadataAndCacheIt(memoryStream);

            Assert.AreEqual(0, _methodCache.Cache.Count);
            Assert.AreEqual(0, _classCache.Cache.Count);
            Assert.AreEqual(0, _moduleCache.Cache.Count);
            Assert.AreEqual(0, _assemblyCache.Cache.Count);
        }
    }
}