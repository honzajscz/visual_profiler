using System;
using System.IO;
using System.Text;
using VisualProfilerAccess.Metadata;

namespace VisualProfilerAccess.ProfilingData.CallTreeElems
{
    public abstract class CallTreeElem
    {
        public UInt32 FunctionId { get; set; }
        public UInt32 ChildrenCount { get; set; }
        public CallTreeElem ParentElem { get; set; }

        protected abstract void DeserializeFields(Stream byteStream);

        public bool IsRootElem()
        {
            bool isRootElem = FunctionId == 0 && ParentElem == null;
            return isRootElem;
        }

        protected abstract void ToString(StringBuilder stringBuilder);
    }

    public abstract class CallTreeElem<TTreeElem> : CallTreeElem
        where TTreeElem : CallTreeElem<TTreeElem>
    {
        private readonly MetadataCache<MethodMetadata> _methodCache;

        protected CallTreeElem(Stream byteStream, ICallTreeElemFactory<TTreeElem> callTreeElemFactory,
                               MetadataCache<MethodMetadata> methodCache)
        {
            _methodCache = methodCache;
            Deserialize(byteStream, callTreeElemFactory);
            if (!IsRootElem())
            {
                MethodMetadata = methodCache[FunctionId];
            }
        }

        public TTreeElem[] Children { get; set; }
        public MethodMetadata MethodMetadata { get; set; }

        public new TTreeElem ParentElem
        {
            get { return (TTreeElem) base.ParentElem; }
            set { base.ParentElem = value; }
        }

        protected void Deserialize(Stream byteStream, ICallTreeElemFactory<TTreeElem> callTreeElemFactory)
        {
            FunctionId = byteStream.DeserializeUint32();
            DeserializeFields(byteStream);
            ChildrenCount = byteStream.DeserializeUint32();

            Children = new TTreeElem[ChildrenCount];
            for (int i = 0; i < ChildrenCount; i++)
            {
                TTreeElem treeElem = callTreeElemFactory.GetCallTreeElem(byteStream, _methodCache);
                treeElem.ParentElem = (TTreeElem) this;
                Children[i] = treeElem;
            }
        }

        public void ConverToString(StringBuilder stringBuilder, Action<StringBuilder, TTreeElem> lineStringModifier,
                                   string indentation = "", string indentationChars = "   ")
        {
            if (!IsRootElem())
            {
                stringBuilder.Append(indentation);
                ToString(stringBuilder);
                if (lineStringModifier != null)
                {
                    lineStringModifier(stringBuilder, (TTreeElem) this);
                }
            }

            int stackDivisionCount = 0;
            foreach (TTreeElem childTreeElem in Children)
            {
                if (IsRootElem())
                {
                    stringBuilder.AppendLine();
                    stringBuilder.AppendFormat("-------------- Stack division {0} --------------", stackDivisionCount++);
                }
                stringBuilder.AppendLine();
                childTreeElem.ConverToString(stringBuilder, lineStringModifier, indentation + indentationChars);
            }
        }
    }
}