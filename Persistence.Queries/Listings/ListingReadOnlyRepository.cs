using Common.ApplicationSettings;
using Core.Application.Helpers;
using Core.Application.Listings.Queries;
using Core.Application.Listings.Queries.Common;
using Core.Application.Listings.Queries.GetMyActiveListingDetails;
using Core.Application.Listings.Queries.GetMyClosedListingDetails;
using Core.Application.Listings.Queries.GetMyExpiredListingDetails;
using Core.Application.Listings.Queries.GetMyListings;
using Core.Application.Listings.Queries.GetMyNewListingDetails;
using Core.Application.Listings.Queries.GetMyPassiveListingDetails;
using Core.Application.Listings.Queries.GetPublicListingDetails;
using Core.Application.Listings.Queries.GetPublicListings;
using Dapper;
using LanguageExt;
using Microsoft.Extensions.Options;
using Persistence.Queries.Listings.Factory;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Persistence.Queries.Listings
{
    public class ListingReadOnlyRepository : IListingReadOnlyRepository
    {
        private readonly string _connectionString;
        private readonly IListingQueryFactory _queryFactory;

        public ListingReadOnlyRepository(IOptions<ConnectionStrings> connectionStrings, IListingQueryFactory queryFactory)
        {
            _queryFactory = queryFactory ??
                throw new ArgumentNullException(nameof(queryFactory));
            if (connectionStrings == null)
                throw new ArgumentNullException(nameof(connectionStrings));

            _connectionString = connectionStrings.Value.Listings;
        }

        public Option<MyActiveListingDetailsModel> FindMyActive(Guid userId, Guid listingId)
        {
            string sql =
                    @"select
                        al.[id]                 as [Id]
                        ,al.[owner]             as [UserId]
                        ,al.[title]             as [Title]
                        ,al.[material_type_id]  as [MaterialTypeId]
                        ,al.[weight_value]      as [Weight]
                        ,al.[weight_unit]       as [MassUnit]
                        ,al.[description]       as [Description]
                        ,al.[first_name]        as [FirstName]
                        ,al.[last_name]         as [LastName]
                        ,al.[phone_number]      as [Phone]
                        ,al.[company]           as [Company]
                        ,al.[country_code]      as [CountryCode]
                        ,al.[city]              as [City]
                        ,al.[post_code]         as [PostCode]
                        ,al.[address]           as [Address]
                        ,al.[state]             as [State]
                        ,al.[latitude]          as [Latitude]
                        ,al.[longitude]         as [Longitude]
                        ,al.[created_date]      as [CreatedDate]
                        ,al.[expiration_date]   as [ExpirationDate]
                        ,ro.[id]                as [Id]
                        ,ro.[monetary_value]    as [Value]
                        ,ro.[currency_code]     as [CurrencyCode] 
                    from 
                        [dbo].[active_listings] al 
                        left join [dbo].[received_offers] ro 
                            on al.id = ro.active_listing_id
                    where 
                        al.[owner] = @owner
                        and al.[id] = @id";

            var parameters = new
            {
                owner = userId,
                id = listingId
            };

            using (var connection = new SqlConnection(_connectionString))
            {
                MyActiveListingDetailsModel model = null;

                connection.Query<MyActiveListingDetailsModel, OfferDetailsModel, MyActiveListingDetailsModel>(
                    sql,
                    (rawListing, rawOffer) =>
                    {
                        if (model == null)
                        {
                            model = rawListing;
                        }

                        model.ReceivedOffers.Add(rawOffer);

                        return null;
                    },
                    parameters);

                return model != null
                    ? Option<MyActiveListingDetailsModel>.Some(model)
                    : Option<MyActiveListingDetailsModel>.None;
            }
        }

        public Option<MyClosedListingDetailsModel> FindMyClosed(Guid userId, Guid listingId)
        {
            string sql =
                    @"select
                        cl.[id]                             as [Id]
                        ,cl.[owner]                         as [UserId]
                        ,cl.[title]                         as [Title]
                        ,cl.[material_type_id]              as [MaterialTypeId]
                        ,cl.[weight_value]                  as [Weight]
                        ,cl.[weight_unit]                   as [MassUnit]
                        ,cl.[description]                   as [Description]
                        ,cl.[first_name]                    as [FirstName]
                        ,cl.[last_name]                     as [LastName]
                        ,cl.[phone_number]                  as [Phone]
                        ,cl.[company]                       as [Company]
                        ,cl.[country_code]                  as [CountryCode]
                        ,cl.[city]                          as [City]
                        ,cl.[post_code]                     as [PostCode]
                        ,cl.[address]                       as [Address]
                        ,cl.[state]                         as [State]
                        ,cl.[latitude]                      as [Latitude]
                        ,cl.[longitude]                     as [Longitude]
                        ,cl.[created_date]                  as [CreatedDate]
                        ,cl.[closed_on]                     as [ClosedOn]
                        ,cl.[accepted_offer_monetary_value] as [AcceptedOfferValue]
                        ,cl.[accepted_offer_currency_code]  as [AcceptedOfferCurrencyCode]
                        ,ap.[user_id]                       as [Id]
                        ,ap.[first_name]                    as [FirstName]
                        ,ap.[last_name]                     as [LastName]
                        ,ap.[phone_number]                  as [Phone]
                        ,ap.[email]                         as [Email]
                    from 
                        [dbo].[closed_listings] cl 
                        left join [dbo].[active_profiles] ap 
                            on cl.[accepted_offer_owner] = ap.[user_id]
                    where 
                        cl.[owner] = @owner
                        and cl.[id] = @id";

            var parameters = new
            {
                owner = userId,
                id = listingId
            };

            using (var connection = new SqlConnection(_connectionString))
            {

               var model = connection.Query<MyClosedListingDetailsModel, OfferOwnerContactDetailsModel, MyClosedListingDetailsModel>(
                    sql,
                    (closedListingDetails, offerOwnerDetails) =>
                    {
                        closedListingDetails.OfferOwnerContactDetails = offerOwnerDetails;
                        return closedListingDetails;
                    },
                    parameters);

                if ((model != null) && (model.First() != null))
                {
                    return Option<MyClosedListingDetailsModel>.Some(model.First());
                }

                return Option<MyClosedListingDetailsModel>.None;
            }
        }

        public Option<MyExpiredListingDetailsModel> FindMyExpired(Guid userId, Guid listingId)
        {
            throw new NotImplementedException();
        }

        public Option<MyNewListingDetailsModel> FindMyNew(Guid userId, Guid listingId)
        {
            string sql =
                    @"select
                        [id]                as [Id]
                        ,[owner]            as [UserId]
                        ,[title]            as [Title]
                        ,[material_type_id] as [MaterialTypeId]
                        ,[weight_value]     as [Weight]
                        ,[weight_unit]      as [MassUnit]
                        ,[description]      as [Description]
                        ,[first_name]       as [FirstName]
                        ,[last_name]        as [LastName]
                        ,[phone_number]     as [Phone]
                        ,[company]          as [Company]
                        ,[country_code]     as [CountryCode]
                        ,[city]             as [City]
                        ,[post_code]        as [PostCode]
                        ,[address]          as [Address]
                        ,[state]            as [State]
                        ,[latitude]         as [Latitude]
                        ,[longitude]        as [Longitude]
                        ,[created_date]     as [CreatedDate]
                    from 
                        [dbo].[new_listings] 
                    where 
                        [owner] = @owner
                        and [id] = @id";


            var parameters = new
            {
                owner = userId,
                id = listingId
            };

            using (var connection = new SqlConnection(_connectionString))
            {
                var model = connection.QuerySingleOrDefault<MyNewListingDetailsModel>(sql, parameters);

                return model != null
                    ? Option<MyNewListingDetailsModel>.Some(model)
                    : Option<MyNewListingDetailsModel>.None;
            }
        }

        public Option<MyPassiveListingDetailsModel> FindMyPassive(Guid userId, Guid listingId)
        {
            string sql =
                    @"select
                        [id]                    as [Id]
                        ,[owner]                as [UserId]
                        ,[title]                as [Title]
                        ,[material_type_id]     as [MaterialTypeId]
                        ,[weight_value]         as [Weight]
                        ,[weight_unit]          as [MassUnit]
                        ,[description]          as [Description]
                        ,[first_name]           as [FirstName]
                        ,[last_name]            as [LastName]
                        ,[phone_number]         as [Phone]
                        ,[company]              as [Company]
                        ,[country_code]         as [CountryCode]
                        ,[city]                 as [City]
                        ,[post_code]            as [PostCode]
                        ,[address]              as [Address]
                        ,[state]                as [State]
                        ,[latitude]             as [Latitude]
                        ,[longitude]            as [Longitude]
                        ,[created_date]         as [CreatedDate]
                        ,[deactivation_date]    as [DeactivationDate]
                        ,[reason]               as [DeactivationReason]
                    from 
                        [dbo].[passive_listings] 
                    where 
                        [owner] = @owner
                        and [id] = @id";

            var parameters = new
            {
                owner = userId,
                id = listingId
            };

            using (var connection = new SqlConnection(_connectionString))
            {
                var model = connection.QuerySingleOrDefault<MyPassiveListingDetailsModel>(sql, parameters);

                return model != null
                    ? Option<MyPassiveListingDetailsModel>.Some(model)
                    : Option<MyPassiveListingDetailsModel>.None;
            }
        }

        public Option<PublicListingDetailsModel> FindPublic(Guid userId, Guid listingId)
        {
            string sql =
                    @"select
                        al.[id]                 as [Id]
                        ,al.[title]             as [Title]
                        ,al.[material_type_id]  as [MaterialTypeId]
                        ,al.[weight_value]      as [Weight]
                        ,al.[weight_unit]       as [MassUnit]
                        ,al.[description]       as [Description]
                        ,al.[city]              as [City]
                        ,ro.[id]                as [Id]
                        ,ro.[monetary_value]    as [Value]
                        ,ro.[currency_code]     as [CurrencyCode]
                    from 
                        [dbo].[active_listings] al 
                        left join [dbo].[received_offers] ro 
                            on al.[id] = ro.[active_listing_id] and ro.[owner] = @owner
                    where 
                        [owner] != @owner
                        and [id] = @id";

            var parameters = new
            {
                owner = userId,
                id = listingId
            };

            using (var connection = new SqlConnection(_connectionString))
            {
                var model = connection.Query<PublicListingDetailsModel, OfferDetailsModel, PublicListingDetailsModel>(
                     sql,
                     (publicListing, myOffer) =>
                     {
                         publicListing.MyOffer = myOffer;
                         return publicListing;
                     },
                     parameters);

                if ((model != null) && (model.First() != null))
                {
                    return Option<PublicListingDetailsModel>.Some(model.First());
                }

                return Option<PublicListingDetailsModel>.None;
            }
        }

        public PagedList<MyListingModel> GetMy(Guid userId, GetMyListingsQueryParams queryParams)
        {
            string sql = _queryFactory
                .CreateSqlForMyListings(queryParams);
            DynamicParameters parameters = _queryFactory
                .CreateParametersForMyListings(userId, queryParams);

            using (var connection = new SqlConnection(_connectionString))
            {
                int totalCount = -1;

                var listings = connection.Query<MyListingModel, int, MyListingModel>(
                    sql,
                    (listing, totalRowCount) =>
                    {
                        if (totalCount == -1)
                        {
                            totalCount = totalRowCount;
                        }

                        return listing;
                    },
                    parameters,
                    splitOn: "TotalRowCount");

                return new PagedList<MyListingModel>(listings.ToList(),
                    totalCount,
                    queryParams.PageNumber,
                    queryParams.PageSize);
            }
        }

        public PagedList<PublicListingModel> GetPublic(Guid userId, GetPublicListingsQueryParams queryParams)
        {
            string sql = _queryFactory
                .CreateSqlForPublicListings(queryParams);
            DynamicParameters parameters = _queryFactory
                .CreateParametersForPublicListings(userId, queryParams);

            using (var connection = new SqlConnection(_connectionString))
            {
                int totalCount = -1;

                var listings = connection.Query<PublicListingModel, int, PublicListingModel>(
                    sql,
                    (listing, totalRowCount) =>
                    {
                        if (totalCount == -1)
                        {
                            totalCount = totalRowCount;
                        }

                        return listing;
                    },
                    parameters,
                    splitOn: "TotalRowCount");

                return new PagedList<PublicListingModel>(listings.ToList(),
                    totalCount,
                    queryParams.PageNumber,
                    queryParams.PageSize);
            }
        }
    }
}
