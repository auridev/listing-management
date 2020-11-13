using Core.Domain.Common;
using Core.Domain.Listings;
using System;
using System.Collections.Generic;

namespace BusinessLine.Core.Domain.UnitTests.TestMocks
{
    // used only to test abstract listing stuff
    internal class ListingMock : Listing
    {
        public string Name { get; }
        public ListingMock(Guid id,
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
