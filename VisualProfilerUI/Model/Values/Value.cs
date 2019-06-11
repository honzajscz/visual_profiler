using System;
using System.Diagnostics.Contracts;

namespace VisualProfilerUI.Model.Values
{
    public abstract class Value<TValue> : IValue where TValue : IComparable
    {
        public readonly TValue ActualValue;

        protected Value(TValue value)
        {
            ActualValue = value;
        }

        public abstract double ConvertToZeroOneScale(IValue maxValue);

        public virtual int CompareTo(Value<TValue> other)
        {
            Contract.Requires(other != null);
            Contract.Requires(other.GetType() == this.GetType());
            Value<TValue> otherCasted = other as Value<TValue>;

            var compareTo = ActualValue.CompareTo(otherCasted.ActualValue);
            return compareTo;
        }

        public abstract string GetAsString(int divider);
    

        public int CompareTo(IValue other)
        {
            return CompareTo((Value<TValue>)other);
        }
    }


}