namespace VisualProfilerUI.Model.CallTreeConvertors.Sampling
{
    public class SamplingGlobalAggregatedValues
    {
        public double WallClockDurationHns { get; set; }
        public ulong TotalActiveTime { get; set; }
        public uint StackTopOccurrenceCount { get; set; }
        public uint LastProfiledFrameInStackCount { get; set; }
    }
}