using System;
using System.Diagnostics;
using Ninject;
using Ninject.Parameters;
using VisualProfilerAccess.ProfilingData.CallTrees;

namespace VisualProfilerAccess.ProfilingData
{
    public class SamplingProfilerAccess : ProfilerAccess<SamplingCallTree>
    {
        public SamplingProfilerAccess(
            ProcessStartInfo profileeProcessStartInfo,
            TimeSpan profilingDataUpdatePeriod,
            EventHandler<ProfilingDataUpdateEventArgs<SamplingCallTree>> updateCallback
            )
            : base(
                profileeProcessStartInfo,
                ProfilerTypes.SamplingProfiler,
                profilingDataUpdatePeriod,
                Kernel.Get<ProfilerCommunicator<SamplingCallTree>>(new ConstructorArgument("updateCallback",
                                                                                           updateCallback)))
        {
        }
    }
}