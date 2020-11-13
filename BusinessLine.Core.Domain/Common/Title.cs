using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.Common
{
    public sealed class Title : IEquatable<Title>
    {
        public TrimmedString Value { get; }

        private Title() { }
        private Title(TrimmedString value)
        {
            Value = value;
        }

        public static Title Create(string value) 
            => new Title((TrimmedString)value);

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<Title>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Title other)
            => ValueObjectComparer<Title>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<Title>.Instance.GetHashCode();

        public static bool operator ==(Title left, Title right)
            => ValueObjectComparer<Title>.Instance.Equals(left, right);

        public static bool operator !=(Title left, Title right)
            => !(left == right);
    }
}
