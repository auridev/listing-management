using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.Functions;

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

        public static Either<Error, FileName> Create(string value)
            =>
                EnsureNonEmpty(value)
                    .Bind(value => Trim(value))
                    .Bind(value => ConvertToLower(value))
                    .Map(value => new FileName(value));

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<FileName>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] FileName other)
            =>
                ValueObjectComparer<FileName>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<FileName>.Instance.GetHashCode();

        public static bool operator ==(FileName left, FileName right)
            =>
                ValueObjectComparer<FileName>.Instance.Equals(left, right);

        public static bool operator !=(FileName left, FileName right)
            =>
                !(left == right);
    }
}
