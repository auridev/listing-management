using Common.Helpers;
using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;

namespace Core.Application.Listings.Commands.CreateNewListing.Factory
{
    public interface INewListingFactory
    {
        Either<Error, NewListing> Create(Owner owner,
            ListingDetails listingDetails,
            ContactDetails contactDetails,
            LocationDetails locationDetail,
            GeographicLocation geographicLocation,
            DateTimeOffset createdDate);
    }
}