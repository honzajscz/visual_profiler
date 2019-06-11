namespace VisualProfilerUI.Model.Criteria.TracingCriteria
{
    public class TimeActiveCriterion : Criterion
    {
        public TimeActiveCriterion() : base("Active time")
        {
        }

        public override int Divider { get { return 10000; } }

        public override string Unit { get { return "ms"; } }
    }
}