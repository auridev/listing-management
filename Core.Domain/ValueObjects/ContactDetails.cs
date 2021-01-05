using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.ValueObjects
{
    public sealed class ContactDetails : IEquatable<ContactDetails>
    {
        public PersonName PersonName { get; }
        public Phone Phone { get; }

        //// this is to overcome current ORM limitations
        public Company ___efCoreCompany { get; private set; }
        public Option<Company> Company
        {
            get
            {
                return ___efCoreCompany == null ? Option<Company>.None : ___efCoreCompany;
            }
            private set
            {
                value
                    .Some(v => {
                        ___efCoreCompany = v;
                    })
                    .None(() => { 
                        ___efCoreCompany = null; 
                    });
            }
        }

        private ContactDetails() { }

        private ContactDetails(PersonName personName, Option<Company> company, Phone phone)
        {
            PersonName = personName;
            Company = company;
            Phone = phone;
        }

        public static Either<Error, ContactDetails> Create(
            Either<Error, PersonName> personName, 
            Either<Error, Option<Company>> optionalCompany, 
            Either<Error, Phone> phone)
        {
            Either<Error, (PersonName personName, Option<Company> optionalCompany,Phone phone)> combined =
                from pN in personName
                from oC in optionalCompany
                from p in phone
                select (pN, oC, p);

            // my new changes
           return combined.Map((combined) => new ContactDetails(combined.personName, combined.optionalCompany, combined.phone));
        }

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
