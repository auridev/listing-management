using LanguageExt;
using System;

namespace Core.Application.Listings.Queries.GetMyExpiredListingDetails
{
    public interface IGetMyExpiredListingDetailsQuery
    {
        Option<MyExpiredListingDetailsModel> Execute(Guid userId, GetMyExpiredListingDetailsQueryParams queryParams);
    }
}