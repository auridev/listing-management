using Core.Domain.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.Common
{
    public sealed class Subject : IEquatable<Subject>
    {
        public TrimmedString Value { get; }

        private Subject() { }

        private Subject(TrimmedString value)
        {
            Value = value;
        }

        public static Subject Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            value = value.CapitalizeFirstLetter();

            return new Subject((TrimmedString)value);
        }

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<Subject>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Subject other)
            => ValueObjectComparer<Subject>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<Subject>.Instance.GetHashCode();

        public static bool operator ==(Subject left, Subject right)
            => ValueObjectComparer<Subject>.Instance.Equals(left, right);

        public static bool operator !=(Subject left, Subject right)
            => !(left == right);
    }
}
