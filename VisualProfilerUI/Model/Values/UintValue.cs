using System.Diagnostics.Contracts;

namespace VisualProfilerUI.Model.Values
{
    public class UintValue : Value<uint>
    {
        public UintValue(uint value) : base(value)
        {}

        public override double ConvertToZeroOneScale(IValue maxValue)
        {
            Contract.Requires(maxValue != null);
            Contract.Requires(maxValue is UintValue);
            Contract.Ensures(0 <= Contract.Result<double>());
            Contract.Ensures(Contract.Result<double>() <= 1);
            UintValue uintMaxValue = maxValue as UintValue;
            if (uintMaxValue.ActualValue == 0)
                return 0;
            else
                return ActualValue / (double)uintMaxValue.ActualValue;
        }

        public override string GetAsString(int divider)
        {
            Contract.Requires(divider != 0);

            return string.Format("{0:N0}", ActualValue / divider);
        }
    }
}