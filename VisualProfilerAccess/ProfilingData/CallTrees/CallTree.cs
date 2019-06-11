using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using VisualProfilerAccess.Metadata;
using VisualProfilerAccess.ProfilingData.CallTreeElems;

namespace VisualProfilerAccess.ProfilingData.CallTrees
{
    public abstract class CallTree
    {
        public UInt32 ThreadId { get; set; }
        public abstract ProfilingDataTypes ProfilingDataType { get; }

        protected virtual void DeserializeFields(Stream byteStream)
        {
        }

        public virtual void ConvertToString(StringBuilder stringBuilder)
        {
        }
    }

    public abstract class CallTree<TCallTree, TCallTreeElem> : CallTree
        where TCallTree : CallTree<TCallTree, TCallTreeElem>
        where TCallTreeElem : CallTreeElem<TCallTreeElem>
    {
        private readonly MetadataCache<MethodMetadata> _methodCache;

        protected CallTree(Stream byteStream, ICallTreeElemFactory<TCallTreeElem> callTreeElemFactory,
                           MetadataCache<MethodMetadata> methodCache)
        {
            _methodCache = methodCache;
            Deserialize(byteStream, callTreeElemFactory);
        }

        public TCallTreeElem RootElem { get; set; }

        protected void Deserialize(Stream byteStream, ICallTreeElemFactory<TCallTreeElem> callTreeElemFactory)
        {
            var profilingDataType = (ProfilingDataTypes) byteStream.DeserializeUint32();
            Contract.Assume(ProfilingDataType == profilingDataType,
                            "The profiling data type derived from stream does not match the type's one.");

            ThreadId = byteStream.DeserializeUint32();
            DeserializeFields(byteStream);

            TCallTreeElem callTreeElem = callTreeElemFactory.GetCallTreeElem(byteStream, _methodCache);
            callTreeElem.ParentElem = null;
            RootElem = callTreeElem;
        }

        public string ToString(Action<StringBuilder, TCallTreeElem> lineStringModifier)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("Thread Id = {0}, Number of stack divisions = {1}", ThreadId,
                                       RootElem.ChildrenCount);
            stringBuilder.AppendLine();
            ConvertToString(stringBuilder);
            stringBuilder.AppendLine();
            RootElem.ConverToString(stringBuilder, lineStringModifier);

            return stringBuilder.ToString();
        }
    }
}