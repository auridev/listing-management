using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.Functions;

namespace Core.Domain.ValueObjects
{
    public sealed class Subject : IEquatable<Subject>
    {
        public TrimmedString Value { get; }

        private Subject() { }

        private Subject(TrimmedString value)
        {
            Value = value;
        }

        public static Either<Error, Subject> Create(string value)
            =>
                EnsureNonEmpty(value)
                    .Bind(value => CapitalizeFirstLetter(value))
                    .Bind(value => TrimmedString.Create(value))
                    .Map(value => new Subject(value));

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<Subject>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Subject other)
            =>
                ValueObjectComparer<Subject>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<Subject>.Instance.GetHashCode();

        public static bool operator ==(Subject left, Subject right)
            =>
                ValueObjectComparer<Subject>.Instance.Equals(left, right);

        public static bool operator !=(Subject left, Subject right)
            =>
                !(left == right);
    }
}
