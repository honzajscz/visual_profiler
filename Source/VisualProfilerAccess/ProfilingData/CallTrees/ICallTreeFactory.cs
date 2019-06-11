using System.IO;
using VisualProfilerAccess.Metadata;
using VisualProfilerAccess.ProfilingData.CallTreeElems;

namespace VisualProfilerAccess.ProfilingData.CallTrees
{
    public interface ICallTreeFactory<TCallTree> where TCallTree : CallTree
    {
        TCallTree GetCallTree(Stream byteStream, MetadataCache<MethodMetadata> methodCache);
    }

    internal class TracingCallTreeFactory : ICallTreeFactory<TracingCallTree>
    {
        #region ICallTreeFactory<TracingCallTree> Members

        public TracingCallTree GetCallTree(Stream byteStream, MetadataCache<MethodMetadata> methodCache)
        {
            var tracingCallTreeElemFactory = new TracingCallTreeElemFactory();
            var tracingCallTree = new TracingCallTree(byteStream, tracingCallTreeElemFactory, methodCache);
            return tracingCallTree;
        }

        #endregion
    }

    internal class SamplingCallTreeFactory : ICallTreeFactory<SamplingCallTree>
    {
        #region ICallTreeFactory<SamplingCallTree> Members

        public SamplingCallTree GetCallTree(Stream byteStream, MetadataCache<MethodMetadata> methodCache)
        {
            var samplingCallTreeElemFactory = new SamplingCallTreeElemFactory();
            var samplingCallTree = new SamplingCallTree(byteStream, samplingCallTreeElemFactory, methodCache);
            return samplingCallTree;
        }

        #endregion
    }
}