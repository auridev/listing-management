using Core.Application.Helpers;
using System;
using System.Collections.Generic;

namespace Core.Application.Listings.Queries.GetPublicListings
{
    public interface IGetPublicListingsQuery
    {
        PagedList<PublicListingModel> Execute(Guid userId, GetPublicListingsQueryParams queryParams);
    }
}