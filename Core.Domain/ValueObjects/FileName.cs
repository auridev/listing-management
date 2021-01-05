using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.ValueObjects
{
    public sealed class FileName : IEquatable<FileName>
    {
        public string Value { get; }

        private FileName() { }
        private FileName(string value)
        {
            Value = value;
        }

        public static FileName Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            value = value.Trim();

            return new FileName(value.ToLower());
        }

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<FileName>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] FileName other)
            => ValueObjectComparer<FileName>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<FileName>.Instance.GetHashCode();

        public static bool operator ==(FileName left, FileName right)
            => ValueObjectComparer<FileName>.Instance.Equals(left, right);

        public static bool operator !=(FileName left, FileName right)
            => !(left == right);
    }
}
