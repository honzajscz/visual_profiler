using System;
using System.Collections.Generic;
using VisualProfilerAccess.ProfilingData.CallTrees;

namespace VisualProfilerAccess.ProfilingData
{
    public class ProfilingDataUpdateEventArgs : EventArgs
    {
        public ProfilerTypes ProfilerType { get; set; }
        public Actions Action { get; set; }
    }

    public class ProfilingDataUpdateEventArgs<TCallTree> : ProfilingDataUpdateEventArgs where TCallTree : CallTree
    {
        public IEnumerable<TCallTree> CallTrees { get; set; }
    }
}