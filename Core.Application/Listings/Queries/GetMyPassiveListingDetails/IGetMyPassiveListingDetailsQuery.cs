using LanguageExt;
using System;

namespace Core.Application.Listings.Queries.GetMyPassiveListingDetails
{
    public interface IGetMyPassiveListingDetailsQuery
    {
        Option<MyPassiveListingDetailsModel> Execute(Guid userId, Guid listingId);
    }
}