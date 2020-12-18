using Core.Application.Profiles.Queries.GetProfileList;
using Dapper;

namespace Persistence.Queries.Profiles.Factory
{
    public class ProfileQueryFactory : IProfileQueryFactory
    {
        public ProfileQueryFactory()
        {

        }

        public string CreateText(GetProfileListQueryParams queryParams)
        {
            string sql = string.Empty;

            if ((queryParams.IsActive.HasValue) && (queryParams.IsActive == true))
            {
                sql =
                    @"select 
                        [id]            as [Id] 
                        ,[email]        as [Email]
                        ,[first_name]   as [FirstName]
                        ,[last_name]    as [LastName]
                        ,[city]         as [City]
                        ,1              as [IsActive]
                    from 
                        [dbo].[active_profiles]
                    where
                        [email] like concat('%',@search,'%')
                        or [first_name] like concat('%',@search,'%') 
                        or [last_name] like concat('%',@search,'%') 
                        or [city] like concat('%',@search,'%') 
                    order by 
                        [created_date]
                    offset @offset rows
                    fetch next @page_size rows only; 



                    select 
                        count(*)
                    from 
                        [dbo].[active_profiles]
                    where
                        [email] like concat('%',@search,'%')
                        or [first_name] like concat('%',@search,'%') 
                        or [last_name] like concat('%',@search,'%') 
                        or [city] like concat('%',@search,'%') ;";
            }
            else if ((queryParams.IsActive.HasValue) && (queryParams.IsActive == false))
            {
                sql =
                    @"select 
                        [id]            as [Id] 
                        ,[email]        as [Email]
                        ,[first_name]   as [FirstName]
                        ,[last_name]    as [LastName]
                        ,[city]         as [City]
                        ,0              as [IsActive]
                    from 
                        [dbo].[passive_profiles]
                    where
                        [email] like concat('%',@search,'%')
                        or [first_name] like concat('%',@search,'%') 
                        or [last_name] like concat('%',@search,'%') 
                        or [city] like concat('%',@search,'%') 
                    order by 
                        [created_date]
                    offset @offset rows
                    fetch next @page_size rows only; 



                    select 
                        count(*)
                    from 
                        [dbo].[passive_profiles]
                    where
                        [email] like concat('%',@search,'%')
                        or [first_name] like concat('%',@search,'%') 
                        or [last_name] like concat('%',@search,'%') 
                        or [city] like concat('%',@search,'%') ;";
            }
            else
            {
                sql =
                    @";with 
                    profiles ([Id], [Email], [FirstName], [LastName], [City], [IsActive], [CreateDate])
                    as
                    (
                        select 
                            [id]            as [Id] 
                            ,[email]        as [Email]
                            ,[first_name]   as [FirstName]
                            ,[last_name]    as [LastName]
                            ,[city]         as [City]
                            ,1              as [IsActive]
                            ,[created_date] as [CreateDate]
                        from 
                            [dbo].[active_profiles]
                        where
                            [email] like concat('%',@search,'%')
                            or [first_name] like concat('%',@search,'%') 
                            or [last_name] like concat('%',@search,'%') 
                            or [city] like concat('%',@search,'%') 
                        
                        union
                        
                        select 
                            [id]            as [Id] 
                            ,[email]        as [Email]
                            ,[first_name]   as [FirstName]
                            ,[last_name]    as [LastName]
                            ,[city]         as [City]
                            ,0              as [IsActive]
                            ,[created_date] as [CreateDate]
                        from 
                            [dbo].[passive_profiles]
                        where
                            [email] like concat('%',@search,'%')
                            or [first_name] like concat('%',@search,'%') 
                            or [last_name] like concat('%',@search,'%') 
                            or [city] like concat('%',@search,'%') 
                    )
                    select 
                        [Id] 
                        ,[Email]
                        ,[FirstName]
                        ,[LastName]
                        ,[City]
                        ,[IsActive]
                    from
                        profiles
                    order by 
                        [CreateDate]
                    offset @offset rows
                    fetch next @page_size rows only; 


                    ;with profile_counts (total)
                    as
                    (
                        select 
                            count(*)
                        from 
                            [dbo].[active_profiles]
                        where
                            [email] like concat('%',@search,'%')
                            or [first_name] like concat('%',@search,'%') 
                            or [last_name] like concat('%',@search,'%') 
                            or [city] like concat('%',@search,'%') 
                        
                        union
                        
                        select 
                            count(*)
                        from 
                            [dbo].[passive_profiles]
                        where
                            [email] like concat('%',@search,'%')
                            or [first_name] like concat('%',@search,'%') 
                            or [last_name] like concat('%',@search,'%') 
                            or [city] like concat('%',@search,'%') 
                    )
                    select 
                        sum(total)
                    from
                        profile_counts;";
            }

            return sql;
        }

        public DynamicParameters CreateParameters(GetProfileListQueryParams queryParams)
        {
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("search", queryParams.Search);
            dynamicParameters.Add("offset", queryParams.DefaultOffset);
            dynamicParameters.Add("page_size", queryParams.PageSize);

            return dynamicParameters;
        }
    }
}
