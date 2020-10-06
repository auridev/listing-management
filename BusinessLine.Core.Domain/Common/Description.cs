using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace BusinessLine.Core.Domain.Common
{
    public sealed class Description : IEquatable<Description>
    {
        public TrimmedString Value { get; }

        private Description() { }
        private Description(TrimmedString value)
        {
            Value = value;
        }

        public static Description Create(string value) 
            => new Description((TrimmedString)value);

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<Description>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Description other)
            => ValueObjectComparer<Description>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<Description>.Instance.GetHashCode();

        public static bool operator ==(Description left, Description right)
            => ValueObjectComparer<Description>.Instance.Equals(left, right);

        public static bool operator !=(Description left, Description right)
            => !(left == right);
    }
}
