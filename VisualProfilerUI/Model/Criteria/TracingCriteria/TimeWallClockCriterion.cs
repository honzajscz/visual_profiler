namespace VisualProfilerUI.Model.Criteria.TracingCriteria
{
    public class TimeWallClockCriterion : Criterion
    {
        public TimeWallClockCriterion()
            : base("Wall-clock time")
        {
        }

        public override int Divider { get { return 10000; } }

        public override string Unit { get { return "ms"; } }
    }
}