using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.ValueObjects
{
    public sealed class Company : IEquatable<Company>
    {
        public TrimmedString Name { get; }

        private Company() { }

        private Company(TrimmedString name)
        {
            Name = name;
        }

        public static Either<Error, Company> Create(string name)
            =>
                ConvertInputValueToTrimmedString(name)
                    .Bind(name => CreateCompany(name));

        private static Either<Error, TrimmedString> ConvertInputValueToTrimmedString(string value)
             =>
                TrimmedString.Create(value);
        private static Either<Error, Company> CreateCompany(Either<Error, TrimmedString> input)
          =>
              input.Map(value => new Company(value));

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<Company>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Company other)
            =>
                ValueObjectComparer<Company>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<Company>.Instance.GetHashCode();

        public static bool operator ==(Company left, Company right)
            =>
                ValueObjectComparer<Company>.Instance.Equals(left, right);

        public static bool operator !=(Company left, Company right)
            =>
                !(left == right);
    }
}
