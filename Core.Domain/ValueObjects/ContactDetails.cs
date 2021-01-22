using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static LanguageExt.Prelude;

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
                    .Some(v =>
                    {
                        ___efCoreCompany = v;
                    })
                    .None(() =>
                    {
                        ___efCoreCompany = null;
                    });
            }
        }

        private ContactDetails() { }

        private ContactDetails(
            PersonName personName,
            Option<Company> company,
            Phone phone)
        {
            PersonName = personName;
            Company = company;
            Phone = phone;
        }

        public static Either<Error, ContactDetails> Create(string firstName, string lastName, string company, string phone)
        {
            Either<Error, Option<Company>> eitherOptionalCompany = CreateOptionalCompany(company);
            Either<Error, PersonName> eitherPersonName = PersonName.Create(firstName, lastName);
            Either<Error, Phone> eitherPhone = Phone.Create(phone);

            Either<Error, (PersonName personName, Option<Company> optionalCompany, Phone phone)> combined =
                from pn in eitherPersonName
                from oC in eitherOptionalCompany
                from p in eitherPhone
                select (pn, oC, p);

            return
                combined.Map(
                    combined =>
                        new ContactDetails(
                            combined.personName,
                            combined.optionalCompany,
                            combined.phone));
        }

        private static Either<Error, Option<Company>> CreateOptionalCompany(string company)
            =>
                string.IsNullOrWhiteSpace(company)
                    ? Right(Option<Company>.None)           // If string's empty it means there's no company 
                    : ValueObjects.Company.Create(company)  // If not empty, then we try to create the company and then assign the final result based on the Create outcome
                        .Right(value => Right<Error, Option<Company>>(Some(value)))
                        .Left(value => Left<Error, Option<Company>>(value));

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<ContactDetails>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] ContactDetails other)
            =>
                ValueObjectComparer<ContactDetails>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<ContactDetails>.Instance.GetHashCode();

        public static bool operator ==(ContactDetails left, ContactDetails right)
            =>
                ValueObjectComparer<ContactDetails>.Instance.Equals(left, right);

        public static bool operator !=(ContactDetails left, ContactDetails right)
            =>
                !(left == right);
    }
}
