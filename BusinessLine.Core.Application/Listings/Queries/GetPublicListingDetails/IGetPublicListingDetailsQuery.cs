using LanguageExt;
using System;

namespace BusinessLine.Core.Application.Listings.Queries.GetPublicListingDetails
{
    public interface IGetPublicListingDetailsQuery
    {
        Option<PublicListingDetailsModel> Execute(Guid userId, GetPublicListingDetailsQueryParams queryParams);
    }
}