using Core.Domain.ValueObjects;
using Core.Domain.Listings;
using System;
using System.Collections.Generic;

namespace Core.Application.Listings.Commands.CreateNewListing.Factory
{
    public class NewListingFactory : INewListingFactory
    {
        public NewListing Create(Owner owner,
            ListingDetails listingDetails,
            ContactDetails contactDetails,
            LocationDetails locationDetail,
            GeographicLocation geographicLocation,
            DateTimeOffset createdDate)
        {
            return new NewListing(Guid.NewGuid(),
                owner,
                listingDetails,
                contactDetails,
                locationDetail,
                geographicLocation,
                createdDate);
        }
    }
}
