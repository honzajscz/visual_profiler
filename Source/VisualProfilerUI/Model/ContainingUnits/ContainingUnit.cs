using System.Collections.Generic;
using System.Diagnostics.Contracts;
using VisualProfilerUI.Model.CriteriaContexts;
using VisualProfilerUI.Model.Methods;

namespace VisualProfilerUI.Model.ContainingUnits
{
    public interface IContainingUnit
    {
        ICriteriaContext CriteriaContext { get; }
        IEnumerable<Method> ContainedMethods { get; }
        string DisplayName { get; }
        string FullName { get; }
    }

    public abstract class ContainingUnit : IContainingUnit
    {
        protected ContainingUnit(
            ICriteriaContext criteriaContext,
            IEnumerable<Method> containedMethods,
            string fullName,
            string displayName,
            int height)
        {
            Contract.Requires(criteriaContext != null);
            Contract.Requires(containedMethods != null);
            CriteriaContext = criteriaContext;
            ContainedMethods = containedMethods;
            FullName = fullName;
            DisplayName = displayName;
            Height = height;
        }

        protected ContainingUnit()
        { }

        public ICriteriaContext CriteriaContext { get; private set; }
        public IEnumerable<Method> ContainedMethods { get; private set; }
        public string DisplayName { get; private set; }
        public string FullName { get; private set; }
        public int Height { get; private set; }
    }
}