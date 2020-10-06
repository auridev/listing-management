using LanguageExt;
using System;

namespace BusinessLine.Core.Application.Listings.Queries.GetMyExpiredListingDetails
{
    public interface IGetMyExpiredListingDetailsQuery
    {
        Option<MyExpiredListingDetailsModel> Execute(Guid userId, GetMyExpiredListingDetailsQueryParams queryParams);
    }
}