using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace BusinessLine.Core.Domain.Common
{
    public sealed class CurrencyCode : IEquatable<CurrencyCode>
    {
        public string Value { get; }

        private CurrencyCode() { }

        private CurrencyCode(string value)
        {
            Value = value;
        }

        public static CurrencyCode Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(nameof(value));

            value = value.Trim().ToUpper();

            if (value.Length != 3)
                throw new ArgumentException(nameof(value));

            return new CurrencyCode(value);
        }

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<CurrencyCode>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] CurrencyCode other)
            => ValueObjectComparer<CurrencyCode>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<CurrencyCode>.Instance.GetHashCode();

        public static bool operator ==(CurrencyCode left, CurrencyCode right)
            => ValueObjectComparer<CurrencyCode>.Instance.Equals(left, right);

        public static bool operator !=(CurrencyCode left, CurrencyCode right)
            => !(left == right);
    }
}
