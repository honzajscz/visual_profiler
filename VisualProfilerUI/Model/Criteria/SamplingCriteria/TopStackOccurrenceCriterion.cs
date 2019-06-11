namespace VisualProfilerUI.Model.Criteria.SamplingCriteria
{
    public class TopStackOccurrenceCriterion : Criterion
    {
        public TopStackOccurrenceCriterion()
            : base("Top stack occurrence")
        {
        }

        public override int Divider
        {
            get { return 1; }
        }

        public override string Unit
        {
            get { return "occurrences"; }
        }
    }
}