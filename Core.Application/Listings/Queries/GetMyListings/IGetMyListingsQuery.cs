using Core.Application.Helpers;
using System;
using System.Collections.Generic;

namespace Core.Application.Listings.Queries.GetMyListings
{
    public interface IGetMyListingsQuery
    {
        PagedList<MyListingModel> Execute(Guid userId, GetMyListingsQueryParams queryParams);
    }
}