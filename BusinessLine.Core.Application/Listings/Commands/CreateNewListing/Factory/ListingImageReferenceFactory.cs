using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Listings;
using System;

namespace BusinessLine.Core.Application.Listings.Commands.CreateNewListing.Factory
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
