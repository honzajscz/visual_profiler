//Include this code in projects that should be profiled.
[assembly: VisualProfiler.ProfilingEnabledAttribute]
namespace VisualProfiler
{
    [System.AttributeUsage(System.AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
    public sealed class ProfilingEnabledAttribute : System.Attribute
    {}
}
