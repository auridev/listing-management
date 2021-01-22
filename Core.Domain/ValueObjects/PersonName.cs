using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.Functions;

namespace Core.Domain.ValueObjects
{
    public sealed class PersonName : IEquatable<PersonName>
    {
        public TrimmedString FirstName { get; }
        public TrimmedString LastName { get; }

        public string FullName
        {
            get => $"{FirstName} {LastName}";
        }

        private PersonName() { }

        private PersonName(TrimmedString firstName, TrimmedString lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public static Either<Error, PersonName> Create(string firstName, string lastName)
        {
            Either<Error, TrimmedString> first =
                EnsureNonEmpty(firstName)
                    .Bind(firstName => CapitalizeAllWords(firstName))
                    .Bind(firstName => TrimmedString.Create(firstName));

            Either<Error, TrimmedString> last =
                EnsureNonEmpty(lastName)
                    .Bind(lastName => CapitalizeAllWords(lastName))
                    .Bind(lastName => TrimmedString.Create(lastName));

            Either<Error, (TrimmedString firstName, TrimmedString lastName)> arguments =
                   from f in first
                   from l in last
                   select (f, l);

            return
                arguments.Map(
                    args => new PersonName(args.firstName, args.lastName));
        }

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<PersonName>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] PersonName other)
            =>
                ValueObjectComparer<PersonName>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<PersonName>.Instance.GetHashCode();

        public static bool operator ==(PersonName left, PersonName right)
            =>
                ValueObjectComparer<PersonName>.Instance.Equals(left, right);

        public static bool operator !=(PersonName left, PersonName right)
            =>
                !(left == right);
    }
}
