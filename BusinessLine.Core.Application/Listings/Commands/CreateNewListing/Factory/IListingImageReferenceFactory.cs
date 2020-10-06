using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Listings;
using System;

namespace BusinessLine.Core.Application.Listings.Commands.CreateNewListing.Factory
{
    public interface IListingImageReferenceFactory
    {
        ListingImageReference Create(Guid parentReference, FileName fileName, FileSize fileSize);
    }
}