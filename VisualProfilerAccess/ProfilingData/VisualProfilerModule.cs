using Ninject.Modules;
using VisualProfilerAccess.Metadata;
using VisualProfilerAccess.ProfilingData.CallTrees;
using VisualProfilerAccess.SourceLocation;

namespace VisualProfilerAccess.ProfilingData
{
    public class VisualProfilerModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ISourceLocatorFactory>().To<SourceLocatorFactory>().InSingletonScope();
            Kernel.Bind<MetadataCache<MethodMetadata>>().ToSelf().InSingletonScope();
            Kernel.Bind<MetadataCache<ClassMetadata>>().ToSelf().InSingletonScope();
            Kernel.Bind<MetadataCache<ModuleMetadata>>().ToSelf().InSingletonScope();
            Kernel.Bind<MetadataCache<AssemblyMetadata>>().ToSelf().InSingletonScope();
            TracingBindings();
            SamplingBindings();
        }

        private void TracingBindings()
        {
            Kernel.Bind<ProfilerTypes>().ToConstant(ProfilerTypes.TracingProfiler).WhenInjectedInto
                <ProfilerCommunicator<TracingCallTree>>();

            Kernel.Bind<ICallTreeFactory<TracingCallTree>>().To<TracingCallTreeFactory>().InSingletonScope();
        }

        private void SamplingBindings()
        {
            Kernel.Bind<ProfilerTypes>().ToConstant(ProfilerTypes.SamplingProfiler).WhenInjectedInto
                <ProfilerCommunicator<SamplingCallTree>>();

            Kernel.Bind<ICallTreeFactory<SamplingCallTree>>().To<SamplingCallTreeFactory>().InSingletonScope();
        }
    }
}