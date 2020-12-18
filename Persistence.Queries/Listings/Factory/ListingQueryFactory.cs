using Core.Application.Listings.Queries.GetMyListings;
using Core.Application.Listings.Queries.GetPublicListings;
using Dapper;
using System;

namespace Persistence.Queries.Listings.Factory
{
    public class ListingQueryFactory : IListingQueryFactory
    {
        public string CreateSqlForMyListings(GetMyListingsQueryParams queryParams)
        {
            var sql =
				@";with 
				my_listings([Id], [Title], [MaterialTypeId], [Type], [CreatedDate])
				as
				(
					select 
						[id]				as [Id]
						,[title]			as [Title]
						,[material_type_id]	as [MaterialTypeId]
						,0					as [Type]
						,[created_date]		as [CreatedDate]
					from 
						[dbo].[new_listings]
					where 
						[owner] = @owner

					union 

					select 
						[id]				as [Id]
						,[title]			as [Title]
						,[material_type_id]	as [MaterialTypeId]
						,1					as [Type]
						,[created_date]		as [CreatedDate]
					from 
						[dbo].[active_listings]
					where 
						[owner] = @owner

					union

					select 
						[id]				as [Id]
						,[title]			as [Title]
						,[material_type_id]	as [MaterialTypeId]
						,2					as [Type]
						,[created_date]		as [CreatedDate]
					from 
						[dbo].[passive_listings]
					where 
						[owner] = @owner

					union

					select 
						[id]				as [Id]
						,[title]			as [Title]
						,[material_type_id]	as [MaterialTypeId]
						,3					as [Type]
						,[created_date]		as [CreatedDate]
					from 
						[dbo].[closed_listings]
					where 
						[owner] = @owner

					union

					select 
						[id]				as [Id]
						,[title]			as [Title]
						,[material_type_id]	as [MaterialTypeId]
						,4					as [Type]
						,[created_date]		as [CreatedDate]
					from 
						[dbo].[suspicious_listings]
					where 
						[owner] = @owner
				),
				my_listing_count([Total])
				as 
				(
					select count(*) from my_listings
		
				)
			select 
				[Id]
				,[Title]
				,[MaterialTypeId]
				,[Type]
				,(select top 1 [Total] from my_listing_count) as [TotalRowCount] 
			from 
				my_listings 
			order by 
				[CreatedDate]
			offset @offset rows
			fetch next @page_size rows only;";

            return sql;
        }

        public DynamicParameters CreateParametersForMyListings(Guid userId, GetMyListingsQueryParams queryParams)
        {
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("owner", userId);
            dynamicParameters.Add("offset", queryParams.DefaultOffset);
			dynamicParameters.Add("page_size", queryParams.PageSize);

            return dynamicParameters;
        }

        public string CreateSqlForPublicListings(GetPublicListingsQueryParams queryParams)
        {
			var join = queryParams.OnlyWithMyOffers
				? " inner join "
				: " left join ";

			var search = !string.IsNullOrWhiteSpace(queryParams.SearchParam)
				? @"and
					(
						[title] like concat('%',@search,'%')
                        or [city] like concat('%',@search,'%') 
                    )"
				: string.Empty;

			var sql =
				$@";with 
				public_listings([Id], [Title], [MaterialTypeId], [Weight], [MassUnit], [City], [HasMyOffer], [CreatedDate])
				as
				(
					select
                        al.[id]                 as [Id]
                        ,al.[title]             as [Title]
                        ,al.[material_type_id]	as [MaterialTypeId]
                        ,al.[weight_value]      as [Weight]
                        ,al.[weight_unit]       as [MassUnit]
                        ,al.[city]              as [City]
						,case
							when ro.[id] is not null then 1
							else 0 
						end						as [HasMyOffer]
						,al.[created_date]		as [CreatedDate]
                    from 
                        [dbo].[active_listings] al { join } [dbo].[received_offers] ro 
							on al.[id] = ro.[active_listing_id] 
							and ro.[owner] = @owner
                    where 
                        al.[owner] != @owner
                        { search }
				),
				public_listing_count([Total])
				as 
				(
					select count(*) from public_listings
				)
				select 
					[Id] 
					,[Title] 
					,[MaterialTypeId]
					,[Weight]
					,[MassUnit]
					,[City]
					,[HasMyOffer]
					,(select top 1 [Total] from public_listing_count) as [TotalRowCount] 
				from 
					public_listings 
				order by 
					[CreatedDate]
				offset @offset rows
				fetch next @page_size rows only;";

            return sql;
        }

        public DynamicParameters CreateParametersForPublicListings(Guid userId, GetPublicListingsQueryParams queryParams)
        {
			var dynamicParameters = new DynamicParameters();
			dynamicParameters.Add("owner", userId);
			dynamicParameters.Add("offset", queryParams.DefaultOffset);
			dynamicParameters.Add("page_size", queryParams.PageSize);
			dynamicParameters.Add("search", queryParams.SearchParam);

			return dynamicParameters;
		}
    }
}
