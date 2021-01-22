using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using System;

namespace Test.Helpers
{
    public class ListingTestFake : Listing
    {
        public string Name { get; }
        public ListingTestFake(Guid id,
            Owner owner,
            ListingDetails listingDetails,
            ContactDetails contactDetails,
            LocationDetails pickupLocationDetails,
            GeographicLocation geographicLocation,
            DateTimeOffset createdDate,
            string name)
            : base(id, owner, listingDetails, contactDetails, pickupLocationDetails, geographicLocation, createdDate)
        {
            Name = name;
        }
    }
}
