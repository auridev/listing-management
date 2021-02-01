using Common.ApplicationSettings;
using Core.Application.Helpers;
using Core.Application.Profiles.Queries;
using Core.Application.Profiles.Queries.GetActiveProfileDetails;
using Core.Application.Profiles.Queries.GetPassiveProfileDetails;
using Core.Application.Profiles.Queries.GetProfileList;
using Core.Application.Profiles.Queries.GetUserProfileDetails;
using Dapper;
using LanguageExt;
using Microsoft.Extensions.Options;
using Persistence.Queries.Profiles.Factory;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Persistence.Queries.Profiles
{
    public class ProfileReadOnlyRepository : IProfileReadOnlyRepository
    {
        private readonly string _connectionString;
        private readonly IProfileQueryFactory _queryTextFactory;

        public ProfileReadOnlyRepository(IOptions<ConnectionStrings> connectionStrings, IProfileQueryFactory queryTextFactory)
        {
            if (connectionStrings == null)
                throw new ArgumentNullException(nameof(connectionStrings));
            _queryTextFactory = queryTextFactory ??
                throw new ArgumentNullException(nameof(queryTextFactory));

            _connectionString = connectionStrings.Value.Listings;
        }

        public Option<ActiveProfileDetailsModel> FindActiveProfile(Guid profileId)
        {
            string sql =
                @"select 
                    [id]                        as [Id]
                    ,[user_id]                  as [UserId]
                    ,[email]                    as [Email]
                    ,[first_name]               as [FirstName]
                    ,[last_name]                as [LastName]
                    ,[company]                  as [Company]
                    ,[phone_number]             as [Phone]
                    ,[country_code]             as [CountryCode]
                    ,[state]                    as [State]
                    ,[city]                     as [City]
                    ,[post_code]                as [PostCode]
                    ,[address]                  as [Address]
                    ,[latitude]                 as [Latitude]
                    ,[longitude]                as [Longitude]
                    ,[distance_unit]            as [DistanceUnit]
                    ,[mass_unit]                as [MassUnit]
                    ,[currency_code]            as [CurrencyCode]
                    ,[introduction_seen_on]     as [IntroductionSeenOn]
                    ,[created_date]             as [CreatedDate]
                from  
                    [dbo].[active_profiles] 
                where
                    [id] = @id";

            var parameters = new { id = profileId };

            using (var connection = new SqlConnection(_connectionString))
            {
                var model = connection.QuerySingleOrDefault<ActiveProfileDetailsModel>(sql, parameters);

                return model != null
                    ? Option<ActiveProfileDetailsModel>.Some(model)
                    : Option<ActiveProfileDetailsModel>.None;
            }
        }

        public Option<PassiveProfileDetailsModel> FindPassiveProfile(Guid profileId)
        {
            string sql =
                @"select 
                    [id]                        as [Id]
                    ,[user_id]                  as [UserId]
                    ,[email]                    as [Email]
                    ,[first_name]               as [FirstName]
                    ,[last_name]                as [LastName]
                    ,[company]                  as [Company]
                    ,[phone_number]             as [Phone]
                    ,[country_code]             as [CountryCode]
                    ,[state]                    as [State]
                    ,[city]                     as [City]
                    ,[post_code]                as [PostCode]
                    ,[address]                  as [Address]
                    ,[latitude]                 as [Latitude]
                    ,[longitude]                as [Longitude]
                    ,[distance_unit]            as [DistanceUnit]
                    ,[mass_unit]                as [MassUnit]
                    ,[currency_code]            as [CurrencyCode]
                    ,[deactivation_date]        as [DeactivationDate]
                    ,[deactivation_reason]      as [DeactivationReason]
                    ,[created_date]             as [CreatedDate]
                from  
                    [dbo].[active_profiles] 
                where
                    [id] = @id";

            var parameters = new { id = profileId };

            using (var connection = new SqlConnection(_connectionString))
            {
                var model = connection.QuerySingleOrDefault<PassiveProfileDetailsModel>(sql, parameters);

                return model != null
                    ? Option<PassiveProfileDetailsModel>.Some(model)
                    : Option<PassiveProfileDetailsModel>.None;
            }
        }
        public Option<UserProfileDetailsModel> FindUserProfile(Guid userId)
        {
            string sql =
                @"select 
                    [id]                as [Id]
                    ,[email]            as [Email]
                    ,[first_name]       as [FirstName]
                    ,[last_name]        as [LastName]
                    ,[company]          as [Company]
                    ,[phone_number]     as [Phone]
                    ,[country_code]     as [CountryCode]
                    ,[state]            as [State]
                    ,[city]             as [City]
                    ,[post_code]        as [PostCode]
                    ,[address]          as [Address]
                    ,[latitude]         as [Latitude]
                    ,[longitude]        as [Longitude]
                    ,[distance_unit]    as [DistanceUnit]
                    ,[mass_unit]        as [MassUnit]
                    ,[currency_code]    as [CurrencyCode]
                    ,case
		                when [introduction_seen_on] is not null then 1
                        else 0 
                    end                 as [IntroductionSeen] 
                    ,[created_date]             as [CreatedDate]
                from  
                    [dbo].[active_profiles] 
                where
                    [user_id] = @user_id";


            var parameters = new { user_id = userId };

            using (var connection = new SqlConnection(_connectionString))
            {
                var model = connection.QuerySingleOrDefault<UserProfileDetailsModel>(sql, parameters);

                return model != null
                    ? Option<UserProfileDetailsModel>.Some(model)
                    : Option<UserProfileDetailsModel>.None;
            }
        }

        public PagedList<ProfileModel> Get(GetProfileListQueryParams queryParams)
        {
            var sql = _queryTextFactory
                .CreateText(queryParams);
            var parameters = _queryTextFactory
                .CreateParameters(queryParams);

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var multipleResults = connection.QueryMultiple(sql, parameters))
                {
                    var items = multipleResults
                        .Read<ProfileModel>()
                        .ToList();

                    var totalCount = multipleResults
                        .ReadFirst<int>();

                    return new PagedList<ProfileModel>(items,
                        totalCount,
                        queryParams.PageNumber,
                        queryParams.PageSize);
                }
            }
        }
    }
}
