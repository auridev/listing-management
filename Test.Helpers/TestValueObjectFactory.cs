using Common.Helpers;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;

namespace Test.Helpers
{
    public static class TestValueObjectFactory
    {
        private static T CreateValueObject<T>(Func<Either<Error, T>> func)
            =>
                func()
                    .Right(value => value)
                    .Left(_ => throw InvalidExecutionPath.Exception);

        public static TrimmedString CreateTrimmedString(string value)
            =>
                CreateValueObject(() => TrimmedString.Create(value));
                
        public static Owner CreateOwner(Guid guid)
            =>
                CreateValueObject(() => Owner.Create(guid));
        
        public static MonetaryValue CreateMonetaryValue(decimal value, string currencyCode)
            =>
                CreateValueObject(() => MonetaryValue.Create(value, currencyCode));

        public static ListingDetails CreateListingDetails(string title, int materialTypeId, float weight, string massUnit, string description)
            =>
                CreateValueObject(() => ListingDetails.Create(title, materialTypeId, weight, massUnit, description));
               
        public static ContactDetails CreateContactDetails(string firstName, string lastName, string company, string phone)
            =>
                CreateValueObject(() => ContactDetails.Create(firstName, lastName, company, phone));

        public static LocationDetails CreateLocationDetails(string alpha2CountryCode, string state, string city, string postCode, string address)
            =>
                CreateValueObject(() => LocationDetails.Create(alpha2CountryCode, state, city, postCode, address));

        public static GeographicLocation CreateGeographicLocation(double latitude, double longitude)
            =>
                 CreateValueObject(() => GeographicLocation.Create(latitude, longitude));

        public static Recipient CreateRecipient(Guid userId)
            =>
                CreateValueObject(() => Recipient.Create(userId));

        public static Subject CreateSubject(string value)
            =>
                CreateValueObject(() => Subject.Create(value));

        public static MessageBody CreateMessageBody(string content)
            =>
                CreateValueObject(() => MessageBody.Create(content));

        public static SeenDate CreateSeenDate(DateTimeOffset value)
            =>
                CreateValueObject(() => SeenDate.Create(value));

        public static Email CreateEmail(string email)
            =>
                CreateValueObject(() => Email.Create(email));

        public static UserPreferences CreateUserPreferences(string distanceUnit, string massUnit, string currencyCode)
            =>
                 CreateValueObject(() => UserPreferences.Create(distanceUnit, massUnit, currencyCode));

        public static FileName CreateFileName(string name)
            =>
                CreateValueObject(() => FileName.Create(name));

        public static FileSize CreateFileSize(long bytes)
            =>
                CreateValueObject(() => FileSize.Create(bytes));
    }
}
