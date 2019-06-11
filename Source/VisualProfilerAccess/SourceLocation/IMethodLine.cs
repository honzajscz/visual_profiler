namespace VisualProfilerAccess.SourceLocation
{
    public interface IMethodLine
    {
        int StartLine { get; }
        int StartColumn { get; }
        int EndColumn { get; }
        int StartIndex { get; }
        int EndIndex { get; }
        int EndLine { get; }
    }
}