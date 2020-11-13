using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.Common
{

    public class SeenDate : IEquatable<SeenDate>
    {
        private static DateTimeOffset _minValue = new DateTimeOffset(DateTime.MinValue, TimeSpan.Zero);

        public virtual DateTimeOffset Value { get; }

        protected SeenDate() { }

        private SeenDate(DateTimeOffset value)
        {
            Value = value;
        }

        public static SeenDate Create(DateTimeOffset? value)
        {
            if (!value.HasValue)
                throw new ArgumentNullException(nameof(value));
            if(value.Value == _minValue)
                throw new ArgumentException(nameof(value));

            return new SeenDate(value.Value);
        }

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<SeenDate>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] SeenDate other)
            => ValueObjectComparer<SeenDate>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<SeenDate>.Instance.GetHashCode();

        public static bool operator ==(SeenDate left, SeenDate right)
            => ValueObjectComparer<SeenDate>.Instance.Equals(left, right);

        public static bool operator !=(SeenDate left, SeenDate right)
            => !(left == right);
    }
}
