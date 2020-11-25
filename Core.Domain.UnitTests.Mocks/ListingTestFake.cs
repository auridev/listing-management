using Core.Domain.Common;
using Core.Domain.Listings;
using System;

namespace Core.UnitTests.Mocks
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
            string name)
            : base(id, owner, listingDetails, contactDetails, pickupLocationDetails, geographicLocation)
        {
            Name = name;
        }
    }
}
