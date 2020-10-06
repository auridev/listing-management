using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Listings;
using System;

namespace BusinessLine.Core.Application.UnitTests.TestMocks
{
    internal class ListingMocks
    {
        public static NewListing NewListing_1 => new NewListing(
            Guid.NewGuid(),
            Owner.Create(Guid.NewGuid()),
            ValueObjectMocks.ListingDetails,
            ValueObjectMocks.ContactDetails,
            ValueObjectMocks.LocationDetails,
            ValueObjectMocks.GeographicLocation,
            DateTimeOffset.UtcNow);

        public static SuspiciousListing SuspiciousListing_1 => new SuspiciousListing(
            Guid.NewGuid(),
            Owner.Create(Guid.NewGuid()),
            ValueObjectMocks.ListingDetails,
            ValueObjectMocks.ContactDetails,
            ValueObjectMocks.LocationDetails,
            ValueObjectMocks.GeographicLocation,
            DateTimeOffset.UtcNow,
            TrimmedString.Create("qwerty"));

        public static PassiveListing PassiveListing_1 => new PassiveListing(
            Guid.NewGuid(),
            Owner.Create(Guid.NewGuid()),
            ValueObjectMocks.ListingDetails,
            ValueObjectMocks.ContactDetails,
            ValueObjectMocks.LocationDetails,
            ValueObjectMocks.GeographicLocation,
            DateTimeOffset.UtcNow,
            TrimmedString.Create("too passive"));

        public static ActiveListing ActiveListing_1 => new ActiveListing(
            Guid.NewGuid(),
            Owner.Create(Guid.Parse("a976bed2-0340-4408-b501-5334d509f11e")),
            ValueObjectMocks.ListingDetails,
            ValueObjectMocks.ContactDetails,
            ValueObjectMocks.LocationDetails,
            ValueObjectMocks.GeographicLocation,
            DateTimeOffset.UtcNow);

        public static Offer Offer_1 = new Offer(
            Guid.Parse("29dcb8ec-2c90-4cfc-8c11-fc5c49d9a43b"),
            Owner.Create(Guid.NewGuid()),
            MonetaryValue.Create(12.5M, CurrencyCode.Create("ASD")),
            DateTimeOffset.UtcNow,
            SeenDate.Create(DateTimeOffset.UtcNow));

        public static Offer Offer_2 = new Offer(
            Guid.Parse("2e565fb4-3a62-48ff-b040-bd30397521ce"),
            Owner.Create(Guid.NewGuid()),
            MonetaryValue.Create(87.2M, CurrencyCode.Create("ASD")),
            DateTimeOffset.UtcNow,
            SeenDate.Create(DateTimeOffset.UtcNow));

        public static Offer Offer_3 = new Offer(
            Guid.Parse("2bfd4922-a20a-4b6a-8ea5-a0450bb5c6eb"),
            Owner.Create(Guid.NewGuid()),
            MonetaryValue.Create(3M, CurrencyCode.Create("ASD")),
            DateTimeOffset.UtcNow,
            SeenDate.Create(DateTimeOffset.UtcNow));

        public static ActiveListing ActiveListing_2 => new ActiveListing(
            Guid.Parse("773dff48-df6e-4829-9c03-b381cbe3d2a3"),
            Owner.Create(Guid.NewGuid()),
            ValueObjectMocks.ListingDetails,
            ValueObjectMocks.ContactDetails,
            ValueObjectMocks.LocationDetails,
            ValueObjectMocks.GeographicLocation,
            DateTimeOffset.UtcNow);
    }
}
