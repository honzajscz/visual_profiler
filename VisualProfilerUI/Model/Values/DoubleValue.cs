using System;
using System.Diagnostics.Contracts;

namespace VisualProfilerUI.Model.Values
{
    public class DoubleValue : Value<double>
    {
        public DoubleValue(double value) 
            : base(value)
        {
        }

        public override double ConvertToZeroOneScale(IValue maxValue)
        {
            Contract.Requires(maxValue != null);
            Contract.Requires(maxValue is DoubleValue);
            Contract.Ensures(0 <= Contract.Result<double>());
            Contract.Ensures(Contract.Result<double>() <= 1);
            double maxValueDouble = (maxValue as DoubleValue).ActualValue;
            bool isZero = Math.Abs(maxValueDouble - 0) <= Double.Epsilon;
            if (isZero)
                return 0;
            else
                return ActualValue / maxValueDouble;
        }

        public override string GetAsString(int divider)
        {
            Contract.Requires(divider != 0);

            return string.Format("{0:N0}", ActualValue / divider);
        }
    }
}