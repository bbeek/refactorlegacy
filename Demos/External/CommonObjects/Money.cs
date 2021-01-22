namespace CommonObjects
{
    public class Money
    {
        public static readonly Money OneThousand = new Money(1000);

        public static readonly Money OneHundred = new Money(100);

        private readonly decimal value;

        public Money(int value) => this.value = value;

        public Money(decimal value) => this.value = value;

        public Money ReduceBy(int p)
        {
            return new Money(value * (100m - p) / 100m);
        }

        public Money Add(Money other)
        {
            return new Money(value + other.value);
        }

        public bool MoreThan(Money other)
        {
            return this.value.CompareTo(other.value) > 0;
        }

        public Money Percentage(int p)
        {
            return new Money(value * p / 100);
        }

        public decimal AsDecimal()
        {
            return value;
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (ReferenceEquals(null, other) || other.GetType() != this.GetType())
            {
                return false;
            }
            var that = (Money)other;
            return this.value.Equals(that.value);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0:0.00}", value);
        }
    }
}
