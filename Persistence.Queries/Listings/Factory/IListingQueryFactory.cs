using Core.Application.Listings.Queries.GetMyListings;
using Core.Application.Listings.Queries.GetPublicListings;
using Dapper;
using System;

namespace Persistence.Queries.Listings.Factory
{
    public interface IListingQueryFactory
    {
        DynamicParameters CreateParametersForMyListings(Guid userId, GetMyListingsQueryParams queryParams);
        DynamicParameters CreateParametersForPublicListings(Guid userId, GetPublicListingsQueryParams queryParams);
        string CreateSqlForMyListings(GetMyListingsQueryParams queryParams);
        string CreateSqlForPublicListings(GetPublicListingsQueryParams queryParams);
    }
}