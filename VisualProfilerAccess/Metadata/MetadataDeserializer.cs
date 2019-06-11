using System;
using System.Diagnostics.Contracts;
using System.IO;
using VisualProfilerAccess.SourceLocation;

namespace VisualProfilerAccess.Metadata
{
    public class MetadataDeserializer
    {
        private readonly MetadataCache<AssemblyMetadata> _assemblyCache;
        private readonly MetadataCache<ClassMetadata> _classCache;
        private readonly MetadataCache<MethodMetadata> _methodCache;
        private readonly MetadataCache<ModuleMetadata> _moduleCache;
        private readonly ISourceLocatorFactory _sourceLocatorFactory;

        public MetadataDeserializer(
            MetadataCache<MethodMetadata> methodCache,
            MetadataCache<ClassMetadata> classCache,
            MetadataCache<ModuleMetadata> moduleCache,
            MetadataCache<AssemblyMetadata> assemblyCache,
            ISourceLocatorFactory sourceLocatorFactory)
        {
            _methodCache = methodCache;
            _classCache = classCache;
            _moduleCache = moduleCache;
            _assemblyCache = assemblyCache;
            _sourceLocatorFactory = sourceLocatorFactory;
        }

        public virtual void DeserializeAllMetadataAndCacheIt(Stream byteStream)
        {
            long initialStreamPostion = byteStream.Position;
            uint metadataByteCount = byteStream.DeserializeUint32();
            long metadataLastBytePosition = metadataByteCount + sizeof (uint) + initialStreamPostion;
            while (byteStream.Position < metadataLastBytePosition)
            {
                MetadataTypes metadataType = byteStream.DeserializeMetadataType();
                MetadataBase result = null;
                switch (metadataType)
                {
                    case MetadataTypes.AssemblyMetadata:
                        var assemblyMetadata = new AssemblyMetadata(byteStream);
                        _assemblyCache.Add(assemblyMetadata);
                        result = assemblyMetadata;
                        break;
                    case MetadataTypes.ModuleMedatada:
                        var moduleMetadata = new ModuleMetadata(byteStream, _assemblyCache);
                        _moduleCache.Add(moduleMetadata);
                        result = moduleMetadata;
                        break;
                    case MetadataTypes.ClassMedatada:
                        var classMetadata = new ClassMetadata(byteStream, _moduleCache);
                        _classCache.Add(classMetadata);
                        result = classMetadata;
                        break;
                    case MetadataTypes.MethodMedatada:
                        var methodMetadata = new MethodMetadata(byteStream, _classCache, _sourceLocatorFactory);
                        _methodCache.Add(methodMetadata);
                        result = methodMetadata;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                Contract.Assume(result != null);
                Contract.Assume(result.Id != 0);
                Contract.Assume(result.MdToken != 0);
                Contract.Assume(metadataType == result.MetadataType);
            }
        }
    }
}