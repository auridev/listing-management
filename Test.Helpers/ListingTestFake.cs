using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using System;

namespace Test.Helpers
{
    public class ListingTestFake : Listing
    {
        public ListingTestFake(Guid id,
            Owner owner,
            ListingDetails listingDetails,
            ContactDetails contactDetails,
            LocationDetails pickupLocationDetails,
            GeographicLocation geographicLocation,
            DateTimeOffset createdDate)
            : base(id, owner, listingDetails, contactDetails, pickupLocationDetails, geographicLocation, createdDate)
        {
        }
    }
}
