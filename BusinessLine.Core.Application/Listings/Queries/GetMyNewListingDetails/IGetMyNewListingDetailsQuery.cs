using LanguageExt;
using System;

namespace BusinessLine.Core.Application.Listings.Queries.GetMyNewListingDetails
{
    public interface IGetMyNewListingDetailsQuery
    {
        Option<MyNewListingDetailsModel> Execute(Guid userId, GetMyNewListingDetailsQueryParams queryParams);
    }
}