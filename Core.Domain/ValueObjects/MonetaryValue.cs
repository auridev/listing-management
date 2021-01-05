using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.ValueObjects
{
    public sealed class MonetaryValue : IEquatable<MonetaryValue>
    {
        public decimal Value { get; }

        public CurrencyCode CurrencyCode { get; }

        private MonetaryValue() { }
        private MonetaryValue(decimal value, CurrencyCode currencyCode)
        {
            Value = value;
            CurrencyCode = currencyCode;
        }

        public static MonetaryValue Create(decimal value, CurrencyCode currencyCode)
        {
            if (value <= 0)
                throw new ArgumentException(nameof(value));
            if (currencyCode == null)
                throw new ArgumentNullException(nameof(currencyCode));

            return new MonetaryValue(value, currencyCode);
        }

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<MonetaryValue>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] MonetaryValue other)
            => ValueObjectComparer<MonetaryValue>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<MonetaryValue>.Instance.GetHashCode();

        public static bool operator ==(MonetaryValue left, MonetaryValue right)
            => ValueObjectComparer<MonetaryValue>.Instance.Equals(left, right);

        public static bool operator !=(MonetaryValue left, MonetaryValue right)
            => !(left == right);
    }
}
