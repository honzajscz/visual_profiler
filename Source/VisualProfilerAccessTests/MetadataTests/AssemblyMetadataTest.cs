using NUnit.Framework;
using VisualProfilerAccess.Metadata;

namespace VisualProfilerAccessTests.MetadataTests
{
    //AssemblyId	0x005c3a58	unsigned int
    //AssemblyMdToken	0x20000001	unsigned int


    [TestFixture]
    public class AssemblyMetadataTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            _assemblyMetadata = new AssemblyMetadata(_rawBytes.ConvertToMemoryStream());
        }

        private readonly byte[] _rawBytes = {
                                                0x00, 0x3A, 0x37, 0x00, 0x01, 0x00, 0x00, 0x20, 0x18, 0x00, 0x00, 0x00,
                                                0x54, 0x00,
                                                0x65, 0x00, 0x73, 0x00, 0x74, 0x00, 0x41, 0x00, 0x73, 0x00, 0x73, 0x00,
                                                0x65, 0x00,
                                                0x6D, 0x00, 0x62, 0x00, 0x6C, 0x00, 0x79, 0x00, 0x01
                                            };

        private const uint ExpectedId = 0x00373a00;
        private const uint ExpectedMdToken = 0x20000001;
        private const string ExpectedName = "TestAssembly";
        private AssemblyMetadata _assemblyMetadata;


        [Test]
        public void IdTest()
        {
            Assert.AreEqual(ExpectedId, _assemblyMetadata.Id, "Assembly id does not match.");
        }

        [Test]
        public void IsProfingEnabledTest()
        {
            Assert.IsTrue(_assemblyMetadata.IsProfilingEnabled, "Assembly should be test profiling enabled.");
        }

        [Test]
        public void MdTokenTest()
        {
            Assert.AreEqual(ExpectedMdToken, _assemblyMetadata.MdToken, "Assembly MdToken does not match.");
        }

        [Test]
        public void MetadataTypeTest()
        {
            Assert.AreEqual(MetadataTypes.AssemblyMetadata, _assemblyMetadata.MetadataType);
        }

        [Test]
        public void NameTest()
        {
            Assert.AreEqual(ExpectedName, _assemblyMetadata.Name, "Assembly name does not match.");
        }
    }
}