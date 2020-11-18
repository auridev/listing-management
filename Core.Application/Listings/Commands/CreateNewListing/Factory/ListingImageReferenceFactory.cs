using Core.Domain.Common;
using Core.Domain.Listings;
using System;

namespace Core.Application.Listings.Commands.CreateNewListing.Factory
{
    public class ListingImageReferenceFactory : IListingImageReferenceFactory
    {
        public ListingImageReference Create(Guid parentReference, FileName fileName, FileSize fileSize)
        {
            return new ListingImageReference(Guid.NewGuid(),
                parentReference,
                fileName,
                fileSize);
        }
    }
}
