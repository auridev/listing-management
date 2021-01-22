using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.Functions;

namespace Core.Domain.ValueObjects
{
    public class SeenDate : IEquatable<SeenDate>
    {
        public virtual DateTimeOffset Value { get; }

        protected SeenDate() { }

        private SeenDate(DateTimeOffset value)
        {
            Value = value;
        }

        public static Either<Error, SeenDate> Create(DateTimeOffset value)
            =>
                EnsureNonDefault(value)
                    .Bind(value => CreateSeenDate(value));

        private static Either<Error, SeenDate> CreateSeenDate(Either<Error, DateTimeOffset> value)
            =>
                value.Map(value => new SeenDate(value));

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<SeenDate>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] SeenDate other)
            =>
                ValueObjectComparer<SeenDate>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<SeenDate>.Instance.GetHashCode();

        public static bool operator ==(SeenDate left, SeenDate right)
            =>
                ValueObjectComparer<SeenDate>.Instance.Equals(left, right);

        public static bool operator !=(SeenDate left, SeenDate right)
            =>
                !(left == right);
    }
}
