using Common.Helpers;
using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using static Common.Helpers.Result;

namespace Core.Application.Listings.Commands.CreateNewListing.Factory
{
    public class NewListingFactory : INewListingFactory
    {
        public Either<Error, NewListing> Create(
            Owner owner,
            ListingDetails listingDetails,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            DateTimeOffset createdDate)
        {
            if (owner == null)
                return Invalid<NewListing>(nameof(owner));
            if (listingDetails == null)
                return Invalid<NewListing>(nameof(listingDetails));
            if (contactDetails == null)
                return Invalid<NewListing>(nameof(contactDetails));
            if (locationDetails == null)
                return Invalid<NewListing>(nameof(locationDetails));
            if (geographicLocation == null)
                return Invalid<NewListing>(nameof(geographicLocation));
            if (createdDate == default)
                return Invalid<NewListing>(nameof(createdDate));

            var listing = new NewListing(
                Guid.NewGuid(),
                owner,
                listingDetails,
                contactDetails,
                locationDetails,
                geographicLocation,
                createdDate);

            return Success(listing);
        }
    }
}
