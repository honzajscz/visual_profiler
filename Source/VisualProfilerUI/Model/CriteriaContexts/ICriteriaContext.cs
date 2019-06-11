using System.Collections.Generic;
using VisualProfilerUI.Model.Criteria;
using VisualProfilerUI.Model.Values;

namespace VisualProfilerUI.Model.CriteriaContexts
{
    public interface ICriteriaContext
    {
        IEnumerable<Criterion> AvailableCriteria { get; }
        IValue GetMaxValueFor(Criterion criterion);
    }
}