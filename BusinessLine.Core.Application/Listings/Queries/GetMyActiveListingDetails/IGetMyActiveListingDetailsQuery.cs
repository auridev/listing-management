using LanguageExt;
using System;

namespace BusinessLine.Core.Application.Listings.Queries.GetMyActiveListingDetails
{
    public interface IGetMyActiveListingDetailsQuery
    {
        Option<MyActiveListingDetailsModel> Execute(Guid userId, GetMyActiveListingDetailsQueryParams queryParams);
    }
}