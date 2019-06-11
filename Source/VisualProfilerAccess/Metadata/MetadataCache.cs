using System.Collections.Generic;

namespace VisualProfilerAccess.Metadata
{
    public class MetadataCache<TMetadata> where TMetadata : MetadataBase
    {
        private Dictionary<uint, TMetadata> _cache = new Dictionary<uint, TMetadata>();

        public virtual Dictionary<uint, TMetadata> Cache
        {
            get { return _cache; }
            set { _cache = value; }
        }

        public virtual TMetadata this[uint metadataId]
        {
            get { return _cache[metadataId]; }
            set { _cache[metadataId] = value; }
        }

        public virtual void Add(TMetadata metadata)
        {
            this[metadata.Id] = metadata;
        }
    }
}