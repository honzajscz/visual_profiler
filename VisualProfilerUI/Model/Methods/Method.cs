using System.Collections.Generic;
using System.Diagnostics.Contracts;
using VisualProfilerUI.Model.Criteria;
using VisualProfilerUI.Model.Values;

namespace VisualProfilerUI.Model.Methods
{
    public abstract class Method //: IMethod
    {
        private readonly IDictionary<Criterion, IValue> _criteriaValuesMap;


        protected Method(uint id,
            string name,
            int firstLineNumber,
            int lineExtend,
            string classFullName,
            string sourceFile)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(firstLineNumber >= 0);
            Contract.Requires(lineExtend > 0);

            Id = id;
            Name = name;
            FirstLineNumber = firstLineNumber;
            LineExtend = lineExtend;
            ClassFullName = classFullName;
            SourceFile = sourceFile;
        }

        public uint Id { get; set; }
        public virtual string Name { get; private set; }

        public virtual IEnumerable<Method> CallingMethods { get; set; }

        public virtual IEnumerable<Method> CalledMethods { get; set; }

        public int FirstLineNumber { get; private set; }

        public int LineExtend { get; private set; }

        public abstract IValue GetValueFor(Criterion criterion);
        public string ClassFullName { get; set; }
        public string SourceFile { get; set; }


    }
}
