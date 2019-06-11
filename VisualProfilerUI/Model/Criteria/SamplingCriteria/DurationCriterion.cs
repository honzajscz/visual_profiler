namespace VisualProfilerUI.Model.Criteria.SamplingCriteria
{
    public class DurationCriterion : Criterion
    {
        public DurationCriterion()
            : base("Duration")
        {
        }

        public override int Divider { get { return 10000; } }

        public override string Unit { get { return "ms"; } }
    }
}