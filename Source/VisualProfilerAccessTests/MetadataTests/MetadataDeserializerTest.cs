﻿using System.IO;
using Moq;
using NUnit.Framework;
using VisualProfilerAccess.Metadata;
using VisualProfilerAccess.SourceLocation;

namespace VisualProfilerAccessTests.MetadataTests
{
    [TestFixture]
    public class MetadataDeserializerTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            _memoryStream = _metadataBytes.ConvertToMemoryStream();
            _methodCache = new MetadataCache<MethodMetadata>();
            _classCache = new MetadataCache<ClassMetadata>();
            _moduleCache = new MetadataCache<ModuleMetadata>();
            _assemblyCache = new MetadataCache<AssemblyMetadata>();


            var mockSourceLocatorFaktory = new Mock<ISourceLocatorFactory>(MockBehavior.Strict);
            var metadataDeserializer = new MetadataDeserializer(
                _methodCache,
                _classCache,
                _moduleCache,
                _assemblyCache,
                mockSourceLocatorFaktory.Object);
            metadataDeserializer.DeserializeAllMetadataAndCacheIt(_memoryStream);
        }

        private readonly byte[] _metadataBytes = {
                                                     0x5C, 0x01, 0x00, 0x00, 0x0B, 0x00, 0x00, 0x00, 0x68, 0x3A, 0x7F,
                                                     0x00, 0x01,
                                                     0x00, 0x00, 0x20, 0x18, 0x00, 0x00, 0x00, 0x54, 0x00, 0x65, 0x00,
                                                     0x73, 0x00,
                                                     0x74, 0x00, 0x41, 0x00, 0x73, 0x00, 0x73, 0x00, 0x65, 0x00, 0x6D,
                                                     0x00, 0x62,
                                                     0x00, 0x6C, 0x00, 0x79, 0x00, 0x01, 0x0C, 0x00, 0x00, 0x00, 0x9C,
                                                     0x2E, 0x15,
                                                     0x00, 0x01, 0x00, 0x00, 0x06,
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
                                                     0x68, 0x3A, 0x7F,
                                                     0x00, 0x0D, 0x00,
                                                     0x00, 0x00,
                                                     0x6C, 0x34, 0x15, 0x00, 0x02, 0x00, 0x00, 0x02, 0x2E, 0x00, 0x00,
                                                     0x00, 0x54,
                                                     0x00, 0x65, 0x00, 0x73, 0x00, 0x74, 0x00, 0x4E, 0x00, 0x61, 0x00,
                                                     0x6D, 0x00,
                                                     0x65, 0x00, 0x73, 0x00, 0x70, 0x00, 0x61, 0x00, 0x63, 0x00, 0x65,
                                                     0x00, 0x2E,
                                                     0x00, 0x54, 0x00, 0x65, 0x00, 0x73, 0x00, 0x74, 0x00, 0x43, 0x00,
                                                     0x6C, 0x00,
                                                     0x61, 0x00, 0x73, 0x00, 0x73, 0x00, 0x00, 0x9C, 0x2E, 0x15, 0x00,
                                                     0x0E, 0x00,
                                                     0x00, 0x00, 0x34, 0x34, 0x15, 0x00, 0x01, 0x00, 0x00, 0x06, 0x08,
                                                     0x00, 0x00,
                                                     0x00, 0x4D, 0x00, 0x61, 0x00, 0x69, 0x00, 0x6E, 0x00, 0x01, 0x00,
                                                     0x00, 0x00,
                                                     0x08, 0x00, 0x00, 0x00, 0x61, 0x00, 0x72, 0x00, 0x67, 0x00, 0x73,
                                                     0x00, 0x6C,
                                                     0x34, 0x15, 0x00, 0x0E, 0x00, 0x00, 0x00, 0x4C, 0x34, 0x15, 0x00,
                                                     0x03, 0x00,
                                                     0x00, 0x06, 0x16, 0x00, 0x00, 0x00, 0x4F, 0x00, 0x74, 0x00, 0x68,
                                                     0x00, 0x65,
                                                     0x00, 0x72, 0x00, 0x4D, 0x00, 0x65, 0x00, 0x74, 0x00, 0x68, 0x00,
                                                     0x6F, 0x00,
                                                     0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x6C, 0x34, 0x15, 0x00, 0x0E,
                                                     0x00, 0x00,
                                                     0x00, 0x58, 0x34, 0x15, 0x00, 0x04, 0x00, 0x00, 0x06, 0x32, 0x00,
                                                     0x00, 0x00,
                                                     0x54, 0x00, 0x65, 0x00, 0x73, 0x00, 0x74, 0x00, 0x4D, 0x00, 0x65,
                                                     0x00, 0x73,
                                                     0x00, 0x73, 0x00, 0x61, 0x00, 0x67, 0x00, 0x65, 0x00, 0x57, 0x00,
                                                     0x69, 0x00,
                                                     0x74, 0x00, 0x68, 0x00, 0x32, 0x00, 0x41, 0x00, 0x72, 0x00, 0x67,
                                                     0x00, 0x75,
                                                     0x00, 0x6D, 0x00, 0x65, 0x00, 0x6E, 0x00, 0x74, 0x00, 0x73, 0x00,
                                                     0x02, 0x00,
                                                     0x00, 0x00, 0x1A, 0x00, 0x00, 0x00, 0x74, 0x00, 0x65, 0x00, 0x73,
                                                     0x00, 0x74,
                                                     0x00, 0x41, 0x00, 0x72, 0x00, 0x67, 0x00, 0x75, 0x00, 0x6D, 0x00,
                                                     0x65, 0x00,
                                                     0x6E, 0x00, 0x74, 0x00, 0x41, 0x00, 0x1A, 0x00, 0x00, 0x00, 0x74,
                                                     0x00, 0x65,
                                                     0x00, 0x73, 0x00, 0x74, 0x00, 0x41, 0x00, 0x72, 0x00, 0x67, 0x00,
                                                     0x75, 0x00,
                                                     0x6D, 0x00, 0x65, 0x00, 0x6E, 0x00, 0x74, 0x00, 0x42, 0x00, 0x6C,
                                                     0x34, 0x15,
                                                     0x00
                                                 };

        private MemoryStream _memoryStream;
        private MetadataCache<MethodMetadata> _methodCache;
        private MetadataCache<ClassMetadata> _classCache;
        private MetadataCache<ModuleMetadata> _moduleCache;
        private MetadataCache<AssemblyMetadata> _assemblyCache;

        [Test]
        public void AllDataReadToEndTest()
        {
            Assert.AreEqual(_memoryStream.Length, _memoryStream.Position);
        }

        [Test]
        public void AssemblyCacheTest()
        {
            Assert.AreEqual(1, _assemblyCache.Cache.Count);
        }

        [Test]
        public void ClassCacheTest()
        {
            Assert.AreEqual(1, _classCache.Cache.Count);
        }

        [Test]
        public void MethodCacheTest()
        {
            Assert.AreEqual(3, _methodCache.Cache.Count);
        }

        [Test]
        public void ModuleCacheTest()
        {
            Assert.AreEqual(1, _moduleCache.Cache.Count);
        }
    }
}