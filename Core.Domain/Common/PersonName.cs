using Core.Domain.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.Common
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

        public static PersonName Create(string firstName, string lastName)
        {
            firstName = firstName.CapitalizeWords();
            lastName = lastName.CapitalizeWords();

            return new PersonName((TrimmedString)firstName, (TrimmedString)lastName);
        }

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<PersonName>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] PersonName other)
            => ValueObjectComparer<PersonName>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<PersonName>.Instance.GetHashCode();

        public static bool operator ==(PersonName left, PersonName right)
            => ValueObjectComparer<PersonName>.Instance.Equals(left, right);

        public static bool operator !=(PersonName left, PersonName right)
            => !(left == right);
    }
}
