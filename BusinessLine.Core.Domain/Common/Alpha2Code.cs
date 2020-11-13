using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.Common
{
    public sealed class Alpha2Code : IEquatable<Alpha2Code>
    {
        public string Value { get; }

        private Alpha2Code() { }

        private Alpha2Code(string value)
        {
            Value = value;
        }

        public static Alpha2Code Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(nameof(value));

            value = value.Trim().ToUpper();

            if (value.Length != 2)
                throw new ArgumentException(nameof(value));

            return new Alpha2Code(value);
        }

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<Alpha2Code>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Alpha2Code other)
            => ValueObjectComparer<Alpha2Code>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<Alpha2Code>.Instance.GetHashCode();

        public static bool operator ==(Alpha2Code left, Alpha2Code right)
            => ValueObjectComparer<Alpha2Code>.Instance.Equals(left, right);

        public static bool operator !=(Alpha2Code left, Alpha2Code right)
            => !(left == right);
    }
}
