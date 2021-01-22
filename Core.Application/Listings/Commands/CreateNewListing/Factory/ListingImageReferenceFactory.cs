using Common.Helpers;
using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using static Common.Helpers.Result;

namespace Core.Application.Listings.Commands.CreateNewListing.Factory
{
    public class ListingImageReferenceFactory : IListingImageReferenceFactory
    {
        public Either<Error, ListingImageReference> Create(Guid parentReference, FileName fileName, FileSize fileSize)
        {
            if (parentReference == default)
                return Invalid<ListingImageReference>(nameof(parentReference));
            if (fileName == null)
                return Invalid<ListingImageReference>(nameof(fileName));
            if (fileSize == null)
                return Invalid<ListingImageReference>(nameof(fileSize));

            var reference = new ListingImageReference(
                Guid.NewGuid(),
                parentReference,
                fileName,
                fileSize);

            return Success(reference);
        }
    }
}
