using LanguageExt;
using System;

namespace Core.Application.Listings.Queries.GetPublicListingDetails
{
    public interface IGetPublicListingDetailsQuery
    {
        Option<PublicListingDetailsModel> Execute(Guid userId, Guid listingId);
    }
}