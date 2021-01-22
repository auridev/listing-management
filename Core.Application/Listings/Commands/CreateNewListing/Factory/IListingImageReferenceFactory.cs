using Common.Helpers;
using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;

namespace Core.Application.Listings.Commands.CreateNewListing.Factory
{
    public interface IListingImageReferenceFactory
    {
        Either<Error, ListingImageReference> Create(Guid parentReference, FileName fileName, FileSize fileSize);
    }
}