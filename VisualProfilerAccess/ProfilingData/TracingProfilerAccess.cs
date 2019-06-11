using System;
using System.Diagnostics;
using Ninject;
using Ninject.Parameters;
using VisualProfilerAccess.ProfilingData.CallTrees;

namespace VisualProfilerAccess.ProfilingData
{
    public class TracingProfilerAccess : ProfilerAccess<TracingCallTree>
    {
        public TracingProfilerAccess(
            ProcessStartInfo profileeProcessStartInfo,
            TimeSpan profilingDataUpdatePeriod,
            EventHandler<ProfilingDataUpdateEventArgs<TracingCallTree>> updateCallback
            )
            : base(
                profileeProcessStartInfo,
                ProfilerTypes.TracingProfiler,
                profilingDataUpdatePeriod,
                Kernel.Get<ProfilerCommunicator<TracingCallTree>>(new ConstructorArgument("updateCallback",
                                                                                          updateCallback)))
        {
        }
    }
}