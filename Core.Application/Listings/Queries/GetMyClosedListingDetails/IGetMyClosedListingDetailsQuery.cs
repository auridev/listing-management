using LanguageExt;
using System;

namespace Core.Application.Listings.Queries.GetMyClosedListingDetails
{
    public interface IGetMyClosedListingDetailsQuery
    {
        Option<MyClosedListingDetailsModel> Execute(Guid userId, Guid listingId);
    }
}