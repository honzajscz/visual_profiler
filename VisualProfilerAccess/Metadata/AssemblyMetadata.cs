using System.IO;

namespace VisualProfilerAccess.Metadata
{
    public class AssemblyMetadata : MetadataBase
    {
        public AssemblyMetadata(Stream byteStream) : base(byteStream)
        {
            Name = byteStream.DeserializeString();
            IsProfilingEnabled = byteStream.DeserializeBool();
        }

        public string Name { get; set; }
        public bool IsProfilingEnabled { get; set; }

        public override MetadataTypes MetadataType
        {
            get { return MetadataTypes.AssemblyMetadata; }
        }
    }
}