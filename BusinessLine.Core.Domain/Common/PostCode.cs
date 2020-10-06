using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace BusinessLine.Core.Domain.Common
{
    public sealed class PostCode : IEquatable<PostCode>
    {
        public TrimmedString Value { get; }

        private PostCode() { }
        private PostCode(TrimmedString value)
        {
            Value = value;
        }

        public static PostCode Create(string value) 
            => new PostCode((TrimmedString)value);

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<PostCode>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] PostCode other)
            => ValueObjectComparer<PostCode>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<PostCode>.Instance.GetHashCode();

        public static bool operator ==(PostCode left, PostCode right)
            => ValueObjectComparer<PostCode>.Instance.Equals(left, right);

        public static bool operator !=(PostCode left, PostCode right)
            => !(left == right);
    }
}
