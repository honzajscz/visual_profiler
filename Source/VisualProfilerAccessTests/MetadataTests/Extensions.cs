using System.IO;

namespace VisualProfilerAccessTests.MetadataTests
{
    internal static class Extensions
    {
        public static MemoryStream ConvertToMemoryStream(this byte[] byteArray)
        {
            var memoryStream = new MemoryStream(byteArray);
            return memoryStream;
        }
    }
}