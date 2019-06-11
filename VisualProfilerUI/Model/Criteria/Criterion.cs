namespace VisualProfilerUI.Model.Criteria
{
    public class Criterion
    {
        public Criterion(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public override bool Equals(object obj)
        {
            return obj is Criterion && Equals((Criterion)obj);
        }

        public bool Equals(Criterion other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name.Equals(other.Name);
        }

        public static bool operator ==(Criterion a, Criterion b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Criterion a, Criterion b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public virtual int Divider { get { return 1; }}

        public virtual string Unit { get { return ""; }
        }
    }
}
