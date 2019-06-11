using System.Collections.Generic;
using VisualProfilerAccess.Metadata;
using VisualProfilerAccess.SourceLocation;

namespace VisualProfilerAccessTests.SourceLocationTests
{
    internal class SrcLocatorMockupFkt : ISourceLocatorFactory
    {
        #region ISourceLocatorFactory Members

        public ISourceLocator GetSourceLocator(string modulePath)
        {
            var srcLocatorMockup = new SrcLocatorMockup(modulePath);
            return srcLocatorMockup;
        }

        public ISourceLocator GetSourceLocator(MethodMetadata methodMd)
        {
            return GetSourceLocator(methodMd.Class.Module.FilePath);
        }

        #endregion
    }

    internal class SrcLocatorMockup : ISourceLocator
    {
        private readonly string _modulePath;

        public SrcLocatorMockup(string modulePath)
        {
            _modulePath = modulePath;
        }

        #region ISourceLocator Members

        public void Dispose()
        {
        }

        public IEnumerable<IMethodLine> GetMethodLines(uint methodMdToken)
        {
            var methodLineMockups = new[] {new MethodLineMockup(), new MethodLineMockup(), new MethodLineMockup()};
            return methodLineMockups;
        }

        public string GetSourceFilePath(uint methodMdToken)
        {
            return _modulePath + ".cs";
        }

        #endregion
    }

    internal class MethodLineMockup : IMethodLine
    {
        #region IMethodLine Members

        public int StartLine
        {
            get { return 111; }
        }

        public int StartColumn
        {
            get { return 9; }
        }

        public int EndColumn
        {
            get { return 23; }
        }

        public int StartIndex
        {
            get { return 1125; }
        }

        public int EndIndex
        {
            get { return 1222; }
        }

        public int EndLine
        {
            get { return 111; }
        }

        #endregion
    }
}