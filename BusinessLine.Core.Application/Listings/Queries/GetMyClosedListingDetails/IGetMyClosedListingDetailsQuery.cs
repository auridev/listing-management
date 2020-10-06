using LanguageExt;
using System;

namespace BusinessLine.Core.Application.Listings.Queries.GetMyClosedListingDetails
{
    public interface IGetMyClosedListingDetailsQuery
    {
        Option<MyClosedListingDetailsModel> Execute(Guid userId, GetMyClosedListingDetailsQueryParams queryParams);
    }
}