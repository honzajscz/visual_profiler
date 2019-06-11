using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using VisualProfilerAccess.SourceLocation;

namespace VisualProfilerAccess.Metadata
{
    public class MethodMetadata : MetadataBase
    {
        private readonly ISourceLocatorFactory _sourceLocatorFactory;

        public MethodMetadata(Stream byteStream, MetadataCache<ClassMetadata> classCache,
                              ISourceLocatorFactory sourceLocatorFactory)
            : base(byteStream)
        {
            Contract.Ensures(classCache != null);
            Contract.Ensures(sourceLocatorFactory != null);
            _sourceLocatorFactory = sourceLocatorFactory;

            Name = byteStream.DeserializeString();

            uint paramCount = byteStream.DeserializeUint32();
            Parameters = new string[paramCount];
            for (int i = 0; i < paramCount; i++)
            {
                string param = byteStream.DeserializeString();
                Parameters[i] = param;
            }

            ClassId = byteStream.DeserializeUint32();
            Class = classCache[ClassId];
        }

        public string Name { get; private set; }
        public string[] Parameters { get; private set; }
        public uint ClassId { get; private set; }

        public ClassMetadata Class { get; set; }

        public override MetadataTypes MetadataType
        {
            get { return MetadataTypes.MethodMedatada; }
        }

        public override string ToString()
        {
            string parametersString = Parameters.Aggregate(string.Empty,
                                                           (current, parameter) => current + (parameter + ", "));
            parametersString = parametersString.TrimEnd(", ".ToCharArray());
            string str = string.Format("[{0}]{1}.{2}({3}) in {4}:{5}:{6}", Class.Module.Assembly.Name, Class, Name,
                                       parametersString, GetSourceFilePath(), GetSourceLocations().First().StartLine,
                                       GetSourceLocations().Last().StartLine);
            return str;
        }

        public string GetSourceFilePath()
        {
            ISourceLocator sourceLocator = _sourceLocatorFactory.GetSourceLocator(this);
            string sourceFilePath = sourceLocator.GetSourceFilePath(MdToken);
            return sourceFilePath;
        }

        public IEnumerable<IMethodLine> GetSourceLocations()
        {
            ISourceLocator sourceLocator = _sourceLocatorFactory.GetSourceLocator(this);
            IEnumerable<IMethodLine> methodLines = sourceLocator.GetMethodLines(MdToken);
            //methodLines.Where()
            return methodLines;
        }
    }
}