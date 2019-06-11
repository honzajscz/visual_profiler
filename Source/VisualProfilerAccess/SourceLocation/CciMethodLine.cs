using Microsoft.Cci;

namespace VisualProfilerAccess.SourceLocation
{
    internal class CciMethodLine : IMethodLine
    {
        public CciMethodLine(IPrimarySourceLocation sourceLocation)
        {
            SourceLocation = sourceLocation;
        }

        public IPrimarySourceLocation SourceLocation { get; set; }

        #region IMethodLine Members

        public int StartLine
        {
            get { return SourceLocation.StartLine; }
        }

        public int EndLine
        {
            get { return SourceLocation.EndLine; }
        }

        public int StartColumn
        {
            get { return SourceLocation.StartColumn; }
        }

        public int EndColumn
        {
            get { return SourceLocation.EndColumn; }
        }

        public int StartIndex
        {
            get { return SourceLocation.StartIndex; }
        }

        public int EndIndex
        {
            get { return SourceLocation.EndIndex; }
        }

        #endregion
    }
}