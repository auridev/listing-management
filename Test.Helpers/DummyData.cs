using Core.Domain.Listings;
using Core.Domain.Messages;
using Core.Domain.Offers;
using Core.Domain.Profiles;
using Core.Domain.ValueObjects;
using System;

namespace Test.Helpers
{
    public class DummyData
    {
        public static Email Email => TestValueObjectFactory.CreateEmail("peter@home.com");

        public static ContactDetails ContactDetails => TestValueObjectFactory.CreateContactDetails("peter", "peterson", "self emplyoment inc", "+111 222 33333");

        public static LocationDetails LocationDetails => TestValueObjectFactory.CreateLocationDetails("GB", "London", "London", "aaa1", "some random place 12");

        public static GeographicLocation GeographicLocation => TestValueObjectFactory.CreateGeographicLocation(10D, 10D);

        public static UserPreferences UserPreferences => TestValueObjectFactory.CreateUserPreferences("m", "lb", "GBP");

        public static ListingDetails ListingDetails => TestValueObjectFactory.CreateListingDetails("very nice listing", 60, 1.2F, "kg", "very very nice");

        public static Recipient Recipient => TestValueObjectFactory.CreateRecipient(Guid.Parse("d1633a55-34fa-4e78-b2c0-348c3951d358"));

        public static Subject Subject => TestValueObjectFactory.CreateSubject("Some important stuff");

        public static MessageBody MessageBody => TestValueObjectFactory.CreateMessageBody("Hey Vladimir, pay attention");

        public static NewListing NewListing_1 => new NewListing(
           Guid.NewGuid(),
           TestValueObjectFactory.CreateOwner(Guid.NewGuid()),
           ListingDetails,
           ContactDetails,
           LocationDetails,
           GeographicLocation,
           DateTimeOffset.UtcNow);

        public static SuspiciousListing SuspiciousListing_1 => new SuspiciousListing(
            Guid.NewGuid(),
            TestValueObjectFactory.CreateOwner(Guid.NewGuid()),
            ListingDetails,
            ContactDetails,
            LocationDetails,
            GeographicLocation,
            DateTimeOffset.UtcNow,
            TestValueObjectFactory.CreateTrimmedString("qwerty"));

        public static PassiveListing PassiveListing_1 => new PassiveListing(
            Guid.NewGuid(),
            TestValueObjectFactory.CreateOwner(Guid.NewGuid()),
            ListingDetails,
            ContactDetails,
            LocationDetails,
            GeographicLocation,
            DateTimeOffset.UtcNow,
            TestValueObjectFactory.CreateTrimmedString("too passive"));

        public static ActiveListing ActiveListing_1 => new ActiveListing(
            Guid.NewGuid(),
            TestValueObjectFactory.CreateOwner(Guid.Parse("a976bed2-0340-4408-b501-5334d509f11e")),
            ListingDetails,
            ContactDetails,
            LocationDetails,
            GeographicLocation,
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow);

        public static ActiveOffer Offer_1 = new ActiveOffer(
            Guid.Parse("29dcb8ec-2c90-4cfc-8c11-fc5c49d9a43b"),
            TestValueObjectFactory.CreateOwner(Guid.NewGuid()),
            TestValueObjectFactory.CreateMonetaryValue(12.5M, "ASD"),
            DateTimeOffset.UtcNow);

        public static ActiveOffer Offer_2 = new ActiveOffer(
            Guid.Parse("2e565fb4-3a62-48ff-b040-bd30397521ce"),
            TestValueObjectFactory.CreateOwner(Guid.NewGuid()),
            TestValueObjectFactory.CreateMonetaryValue(87.2M, "ASD"),
            DateTimeOffset.UtcNow);

        public static ActiveOffer Offer_3 = new ActiveOffer(
            Guid.Parse("2bfd4922-a20a-4b6a-8ea5-a0450bb5c6eb"),
            TestValueObjectFactory.CreateOwner(Guid.NewGuid()),
            TestValueObjectFactory.CreateMonetaryValue(3M, "ASD"),
            DateTimeOffset.UtcNow);

        public static ActiveListing ActiveListing_2 => new ActiveListing(
            Guid.Parse("773dff48-df6e-4829-9c03-b381cbe3d2a3"),
            TestValueObjectFactory.CreateOwner(Guid.NewGuid()),
            ListingDetails,
            ContactDetails,
            LocationDetails,
            GeographicLocation,
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow);

        public static Message Message_1 => new Message(
            Guid.NewGuid(),
            Recipient,
            Subject,
            MessageBody,
            DateTimeOffset.UtcNow);

        public static ActiveProfile UK_Profile => new ActiveProfile(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Email,
            ContactDetails,
            LocationDetails,
            GeographicLocation,
            UserPreferences,
            DateTimeOffset.UtcNow);
    }
}
