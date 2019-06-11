using System;
using System.Diagnostics.Contracts;

namespace VisualProfilerUI.Model.Values
{
    public class Uint64Value : Value<ulong>
    {
        public Uint64Value(UInt64 value)
            : base(value)
        { }

        public override double ConvertToZeroOneScale(IValue maxValue)
        {
            Contract.Requires(maxValue != null);
            Contract.Requires(maxValue is Uint64Value);
            Contract.Ensures(0 <= Contract.Result<double>());
            Contract.Ensures(Contract.Result<double>() <= 1);
            Uint64Value uint64MaxValue = (Uint64Value) maxValue ;
            if (uint64MaxValue.ActualValue == 0)
                return 0;
            else
                return ActualValue / (double)uint64MaxValue.ActualValue;
        }

        public override string GetAsString(int divider)
        {
            Contract.Requires(divider != 0);

            return string.Format("{0:N0}", ActualValue / (ulong)divider);
        }
    }
}