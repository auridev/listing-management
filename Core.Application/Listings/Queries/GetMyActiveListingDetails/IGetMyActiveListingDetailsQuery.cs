using LanguageExt;
using System;

namespace Core.Application.Listings.Queries.GetMyActiveListingDetails
{
    public interface IGetMyActiveListingDetailsQuery
    {
        Option<MyActiveListingDetailsModel> Execute(Guid userId, Guid listingId);
    }
}