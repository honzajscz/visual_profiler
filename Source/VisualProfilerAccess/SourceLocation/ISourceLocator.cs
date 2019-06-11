using System;
using System.Collections.Generic;

namespace VisualProfilerAccess.SourceLocation
{
    public interface ISourceLocator : IDisposable
    {
        IEnumerable<IMethodLine> GetMethodLines(uint methodMdToken);
        string GetSourceFilePath(uint methodMdToken);
    }
}