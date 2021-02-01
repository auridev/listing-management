using Common.ApplicationSettings;
using Core.Application.Helpers;
using Core.Application.Messages.Queries;
using Core.Application.Messages.Queries.GetMyMessageDetails;
using Core.Application.Messages.Queries.GetMyMessages;
using Dapper;
using LanguageExt;
using Microsoft.Extensions.Options;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Persistence.Queries.Messages
{
    public class MessageReadOnlyRepository : IMessageReadOnlyRepository
    {
        private readonly string _connectionString;

        public MessageReadOnlyRepository(IOptions<ConnectionStrings> connectionStrings)
        {
            if (connectionStrings == null)
                throw new ArgumentNullException(nameof(connectionStrings));

            _connectionString = connectionStrings.Value.Listings;
        }

        public Option<MyMessageDetailsModel> Find(Guid userId, Guid messageId)
        {
            var sql =
                @"select 
                    [id]			as [Id]
	                ,[subject]		as [Subject]
                    ,[body]			as [Body]
                    ,[created_date]	as [CreatedDate]
	                ,case
		                when [seen_date] is not null then 1
                        else 0 
                    end             as [Seen] 
                from 
	                [dbo].[messages]
                where
                    [id] = @id 
                    and [recipient] = @recipient";

            var parameters = new { id = messageId, recipient = userId };

            using (var connection = new SqlConnection(_connectionString))
            {
                var model = connection.QuerySingleOrDefault<MyMessageDetailsModel>(sql, parameters);

                return model != null 
                    ? Option<MyMessageDetailsModel>.Some(model) 
                    : Option<MyMessageDetailsModel>.None;
            }
        }

        public PagedList<MyMessageModel> Get(Guid userId, GetMyMessagesQueryParams queryParams)
        {
            var sql =
                @"select 
                    [id]			as [Id]
	                ,[subject]		as [Subject]
                    ,[created_date]	as [CreatedDate]
                from 
	                [dbo].[messages]
                where
                    [recipient] = @recipient
                order by 
                    created_date
                offset @offset rows
                fetch next @page_size rows only; 


                select 
                    count(*)
                from 
	                [dbo].[messages]
                where
                    [recipient] = @recipient;";

            var parameters = new
            {
                recipient = userId,
                offset = queryParams.DefaultOffset,
                page_size = queryParams.PageSize
            };

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var multipleResults = connection.QueryMultiple(sql, parameters))
                {
                    var items = multipleResults
                        .Read<MyMessageModel>()
                        .ToList();

                    var totalCount = multipleResults
                        .ReadFirst<int>();

                    return new PagedList<MyMessageModel>(items, 
                        totalCount, 
                        queryParams.PageNumber, 
                        queryParams.PageSize);
                }
            }
        }
    }
}
