using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace BusinessLine.Core.Domain.Common
{
    public sealed class Alpha3Code : IEquatable<Alpha3Code>
    {
        public string Value { get; }

        private Alpha3Code() { }

        private Alpha3Code(string value)
        {
            Value = value;
        }

        public static Alpha3Code Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(nameof(value));

            value = value.Trim().ToUpper();

            if (value.Length != 3)
                throw new ArgumentException(nameof(value));

            return new Alpha3Code(value);
        }

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<Alpha3Code>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Alpha3Code other)
            => ValueObjectComparer<Alpha3Code>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<Alpha3Code>.Instance.GetHashCode();

        public static bool operator ==(Alpha3Code left, Alpha3Code right)
            => ValueObjectComparer<Alpha3Code>.Instance.Equals(left, right);

        public static bool operator !=(Alpha3Code left, Alpha3Code right)
            => !(left == right);
    }
}
