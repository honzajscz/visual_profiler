using System;
using System.IO;
using NUnit.Framework;
using VisualProfilerAccess;
using VisualProfilerAccess.Metadata;
using VisualProfilerAccessTests.MetadataTests;

namespace VisualProfilerAccessTests
{
    [TestFixture]
    public class DeserializationExtensionsTests
    {
        [Test]
        public void DeserializeBoolTest()
        {
            byte[] bytes = {0x00, 0x01};
            MemoryStream memoryStream = bytes.ConvertToMemoryStream();

            bool falseValue = memoryStream.DeserializeBool();
            Assert.IsFalse(falseValue);

            bool trueValue = memoryStream.DeserializeBool();
            Assert.IsTrue(trueValue);
        }

        [Test]
        public void DeserializeMetadataTypesTest()
        {
            byte[] bytes = {
                               0x0B, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x0E, 0x00, 0x00,
                               0x00
                           };
            MemoryStream memoryStream = bytes.ConvertToMemoryStream();

            MetadataTypes metadataType = memoryStream.DeserializeMetadataType();
            Assert.AreEqual(MetadataTypes.AssemblyMetadata, metadataType);

            metadataType = memoryStream.DeserializeMetadataType();
            Assert.AreEqual(MetadataTypes.ModuleMedatada, metadataType);

            metadataType = memoryStream.DeserializeMetadataType();
            Assert.AreEqual(MetadataTypes.ClassMedatada, metadataType);

            metadataType = memoryStream.DeserializeMetadataType();
            Assert.AreEqual(MetadataTypes.MethodMedatada, metadataType);
        }

        [Test]
        public void DeserializeStringTest()
        {
            byte[] bytes = {
                               0x50, 0x00, 0x00, 0x00, 0x54, 0x00, 0x68, 0x00, 0x69, 0x00, 0x73, 0x00, 0x20, 0x00, 0x69,
                               0x00, 0x73, 0x00, 0x20, 0x00, 0x61, 0x00, 0x20, 0x00, 0x74, 0x00, 0x65, 0x00, 0x73, 0x00,
                               0x74, 0x00, 0x20, 0x00, 0x73, 0x00, 0x74, 0x00, 0x72, 0x00, 0x69, 0x00, 0x6E, 0x00, 0x67,
                               0x00, 0x21, 0x00, 0x20, 0x00, 0x31, 0x00, 0x32, 0x00, 0x33, 0x00, 0x34, 0x00, 0x35, 0x00,
                               0x36, 0x00, 0x20, 0x00, 0x21, 0x00, 0x40, 0x00, 0x23, 0x00, 0x24, 0x00, 0x25, 0x00, 0x5E,
                               0x00, 0x5B, 0x00, 0x5D, 0x00, 0x2B, 0x00, 0x5F, 0x00
                           };
            string expectedString = "This is a test string! 123456 !@#$%^[]+_";
            string actualString = bytes.ConvertToMemoryStream().DeserializeString();
            Assert.AreEqual(expectedString, actualString);
        }

        [Test]
        public void DeserializeUInt32Test()
        {
            byte[] bytes = {0x40, 0xE2, 0x01, 0x00};

            uint expectedUint32 = 123456; //0x001E240
            uint actualUint32 = bytes.ConvertToMemoryStream().DeserializeUint32();

            Assert.AreEqual(expectedUint32, actualUint32);
        }

        [Test]
        public void DeserializeUInt64Test()
        {
            byte[] bytes = {0xF0, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12};
            UInt64 expectedValue = 0x123456789abcdef0;
            UInt64 actualValue = bytes.ConvertToMemoryStream().DeserializeUInt64();
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}