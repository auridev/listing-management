using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace BusinessLine.Core.Domain.Common
{
    public sealed class ContactDetails : IEquatable<ContactDetails>
    {
        public PersonName PersonName { get; }
        public Company Company { get; }
        public Phone Phone { get; }

        private ContactDetails() { }

        private ContactDetails(PersonName personName, Company company, Phone phone)
        {
            PersonName = personName;
            Company = company;
            Phone = phone;
        }

        public static ContactDetails Create(PersonName personName, Company company, Phone phone)
            => new ContactDetails(personName, company, phone);

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<ContactDetails>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] ContactDetails other)
            => ValueObjectComparer<ContactDetails>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<ContactDetails>.Instance.GetHashCode();

        public static bool operator ==(ContactDetails left, ContactDetails right)
            => ValueObjectComparer<ContactDetails>.Instance.Equals(left, right);

        public static bool operator !=(ContactDetails left, ContactDetails right)
            => !(left == right);
    }
}
