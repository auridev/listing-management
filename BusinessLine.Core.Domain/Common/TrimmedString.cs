using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.Common
{
    public sealed class TrimmedString: IEquatable<TrimmedString>
    {
        public string Value { get; }

        private TrimmedString() { }

        private TrimmedString(string value)
        {
            Value = value;
        }

        public static TrimmedString Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(nameof(value));

            value = value.Trim();

            return new TrimmedString(value);
        }

        public override string ToString()
            => Value;

        public static implicit operator string(TrimmedString trimmedString)
            => trimmedString.Value;

        public static explicit operator TrimmedString(string str)
            => Create(str);

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<TrimmedString>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] TrimmedString other)
            => ValueObjectComparer<TrimmedString>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<TrimmedString>.Instance.GetHashCode();

        public static bool operator ==(TrimmedString left, TrimmedString right)
            => ValueObjectComparer<TrimmedString>.Instance.Equals(left, right);

        public static bool operator !=(TrimmedString left, TrimmedString right)
            => !(left == right);
    }
}
