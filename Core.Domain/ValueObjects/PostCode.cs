using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.ValueObjects
{
    public sealed class PostCode : IEquatable<PostCode>
    {
        public TrimmedString Value { get; }

        private PostCode() { }
        private PostCode(TrimmedString value)
        {
            Value = value;
        }

        public static Either<Error, PostCode> Create(string value)
            =>
                ConvertToTrimmedString(value)
                    .Bind(value => CreatePostCode(value));

        private static Either<Error, TrimmedString> ConvertToTrimmedString(string value)
            =>
                TrimmedString.Create(value);

        private static Either<Error, PostCode> CreatePostCode(Either<Error, TrimmedString> value)
            =>
                value.Map(value => new PostCode(value));

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<PostCode>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] PostCode other)
            =>
                ValueObjectComparer<PostCode>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<PostCode>.Instance.GetHashCode();

        public static bool operator ==(PostCode left, PostCode right)
            =>
                ValueObjectComparer<PostCode>.Instance.Equals(left, right);

        public static bool operator !=(PostCode left, PostCode right)
            =>
                !(left == right);
    }
}
