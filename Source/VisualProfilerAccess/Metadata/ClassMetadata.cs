using System.Diagnostics.Contracts;
using System.IO;

namespace VisualProfilerAccess.Metadata
{
    public class ClassMetadata : MetadataBase
    {
        public ClassMetadata(Stream byteStream, MetadataCache<ModuleMetadata> moduleCache) : base(byteStream)
        {
            Contract.Ensures(moduleCache != null);
            Name = byteStream.DeserializeString();
            IsGeneric = byteStream.DeserializeBool();
            ModuleId = byteStream.DeserializeUint32();
            Module = moduleCache[ModuleId];
        }

        public string Name { get; private set; }
        public bool IsGeneric { get; private set; }
        public ModuleMetadata Module { get; set; }
        public uint ModuleId { get; private set; }

        public override MetadataTypes MetadataType
        {
            get { return MetadataTypes.ClassMedatada; }
        }

        public override string ToString()
        {
            string className = Name + (IsGeneric ? "<>" : string.Empty);
            return className;
        }
    }
}