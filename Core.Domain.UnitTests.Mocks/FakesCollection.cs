using Core.Domain.ValueObjects;
using Core.Domain.Listings;
using Core.Domain.Messages;
using Core.Domain.Offers;
using Core.Domain.Profiles;
using LanguageExt;
using System;
using Common.Helpers;

namespace Core.UnitTests.Mocks
{
    public class FakesCollection
    {
        public static Email Email => Email.Create("peter@home.com");

        public static ContactDetails ContactDetails => ContactDetails.Create(PersonName.Create("peter", "peterson"),
                Company.Create("self emplyoment inc"),
                Phone.Create("+111 222 33333"));

        public static LocationDetails LocationDetails => LocationDetails.Create(
            Alpha2Code.Create("GB").ToUnsafeRight(),
            State.Create("London"),
            City.Create("London").ToUnsafeRight(),
            PostCode.Create("aaa1"),
            Address.Create("some random place 12").ToUnsafeRight());

        public static GeographicLocation GeographicLocation => GeographicLocation.Create(10D, 10D);

        public static UserPreferences UserPreferences => UserPreferences.Create(
                DistanceMeasurementUnit.Mile,
                MassMeasurementUnit.Pound,
                CurrencyCode.Create("GBP"));

        public static ListingDetails ListingDetails => ListingDetails.Create(
            Title.Create("very nice listing"),
            MaterialType.Electronics,
            Weight.Create(1.2F, MassMeasurementUnit.Kilogram),
            Description.Create("very very nice"));

        public static Recipient Recipient => Recipient.Create(Guid.Parse("d1633a55-34fa-4e78-b2c0-348c3951d358"));

        public static Subject Subject => Subject.Create("Some important stuff");

        public static MessageBody MessageBody => MessageBody.Create("Hey Vladimir, pay attention");

        public static NewListing NewListing_1 => new NewListing(
           Guid.NewGuid(),
           Owner.Create(Guid.NewGuid()),
           ListingDetails,
           ContactDetails,
           LocationDetails,
           GeographicLocation,
           DateTimeOffset.UtcNow);

        public static SuspiciousListing SuspiciousListing_1 => new SuspiciousListing(
            Guid.NewGuid(),
            Owner.Create(Guid.NewGuid()),
            ListingDetails,
            ContactDetails,
            LocationDetails,
            GeographicLocation,
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow,
            TrimmedString.Create("qwerty").ToUnsafeRight());

        public static PassiveListing PassiveListing_1 => new PassiveListing(
            Guid.NewGuid(),
            Owner.Create(Guid.NewGuid()),
            ListingDetails,
            ContactDetails,
            LocationDetails,
            GeographicLocation,
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow,
            TrimmedString.Create("too passive").ToUnsafeRight());

        public static ActiveListing ActiveListing_1 => new ActiveListing(
            Guid.NewGuid(),
            Owner.Create(Guid.Parse("a976bed2-0340-4408-b501-5334d509f11e")),
            ListingDetails,
            ContactDetails,
            LocationDetails,
            GeographicLocation,
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow);

        public static ReceivedOffer Offer_1 = new ReceivedOffer(
            Guid.Parse("29dcb8ec-2c90-4cfc-8c11-fc5c49d9a43b"),
            Owner.Create(Guid.NewGuid()),
            MonetaryValue.Create(12.5M, CurrencyCode.Create("ASD")),
            DateTimeOffset.UtcNow,
            SeenDate.Create(DateTimeOffset.UtcNow));

        public static ReceivedOffer Offer_2 = new ReceivedOffer(
            Guid.Parse("2e565fb4-3a62-48ff-b040-bd30397521ce"),
            Owner.Create(Guid.NewGuid()),
            MonetaryValue.Create(87.2M, CurrencyCode.Create("ASD")),
            DateTimeOffset.UtcNow,
            SeenDate.Create(DateTimeOffset.UtcNow));

        public static ReceivedOffer Offer_3 = new ReceivedOffer(
            Guid.Parse("2bfd4922-a20a-4b6a-8ea5-a0450bb5c6eb"),
            Owner.Create(Guid.NewGuid()),
            MonetaryValue.Create(3M, CurrencyCode.Create("ASD")),
            DateTimeOffset.UtcNow,
            SeenDate.Create(DateTimeOffset.UtcNow));

        public static ActiveListing ActiveListing_2 => new ActiveListing(
            Guid.Parse("773dff48-df6e-4829-9c03-b381cbe3d2a3"),
            Owner.Create(Guid.NewGuid()),
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
            Option<SeenDate>.None,
            DateTimeOffset.UtcNow);

        public static ActiveProfile UK_Profile => new ActiveProfile(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Email,
            ContactDetails,
            LocationDetails,
            GeographicLocation,
            UserPreferences,
            DateTimeOffset.UtcNow,
            Option<SeenDate>.None);
    }
}
