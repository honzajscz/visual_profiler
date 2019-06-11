using VisualProfilerAccess.Metadata;

namespace VisualProfilerAccess.SourceLocation
{
    public interface ISourceLocatorFactory
    {
        ISourceLocator GetSourceLocator(string modulePath);
        ISourceLocator GetSourceLocator(MethodMetadata methodMd);
    }
}