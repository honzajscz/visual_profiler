using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace VisualProfilerAccess.Metadata
{
    public abstract class MetadataBase : IEquatable<MetadataBase>
    {
        public uint Id { get; set; }
        public uint MdToken { get; set; }

        public abstract MetadataTypes MetadataType { get; }

        public bool Equals(MetadataBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id == Id;
        }

        protected MetadataBase(Stream byteStream) : this(
            byteStream.DeserializeUint32(),
            byteStream.DeserializeUint32())
        {
            Contract.Requires(byteStream != null);
        }

        protected MetadataBase(uint id, uint mdToken)
        {
            Id = id;
            MdToken = mdToken;
        }
        
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}