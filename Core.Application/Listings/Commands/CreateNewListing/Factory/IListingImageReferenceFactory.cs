using Core.Domain.ValueObjects;
using Core.Domain.Listings;
using System;

namespace Core.Application.Listings.Commands.CreateNewListing.Factory
{
    public interface IListingImageReferenceFactory
    {
        ListingImageReference Create(Guid parentReference, FileName fileName, FileSize fileSize);
    }
}