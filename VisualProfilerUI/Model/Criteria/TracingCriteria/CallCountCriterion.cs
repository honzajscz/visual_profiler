namespace VisualProfilerUI.Model.Criteria.TracingCriteria
{
    public class CallCountCriterion : Criterion
    {
        public CallCountCriterion()
            : base("Call count")
        {
        }

        public override int Divider { get { return 1; } }

        public override string Unit { get { return "calls"; } }
    }
}