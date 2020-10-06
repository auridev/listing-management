using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Listings;
using System;
using System.Collections.Generic;

namespace BusinessLine.Core.Application.Listings.Commands.CreateNewListing.Factory
{
    public interface INewListingFactory
    {
        NewListing Create(Owner owner, 
            ListingDetails listingDetails, 
            ContactDetails contactDetails, 
            LocationDetails locationDetail, 
            GeographicLocation geographicLocation,
            DateTimeOffset createdDate);
    }
}