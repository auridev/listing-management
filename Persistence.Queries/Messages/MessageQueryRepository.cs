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
    public class MessageQueryRepository : IMessageQueryRepository
    {
        private readonly string _connectionString;

        public MessageQueryRepository(IOptions<ConnectionStrings> connectionStrings)
        {
            if (connectionStrings == null)
                throw new ArgumentNullException(nameof(connectionStrings));

            _connectionString = connectionStrings.Value.BusinessLine;
        }

        public Option<MyMessageDetailsModel> Find(Guid userId, GetMyMessageDetailsQueryParams queryParams)
        {
            var sql =
                @"select 
                    [id]			as [Id],
	                [subject]		as [Subject],
                    [body]			as [Body],
                    [created_date]	as [CreatedDate],
	                case
		                when [seen_date] is not null then 1
                        else 0 
                    end as [Seen] 
                from 
	                [dbo].[messages]
                where
                    [id] = @id 
                    and [recipient] = @recipient";

            var parameters = new { id = queryParams.MessageId, recipient = userId };

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QuerySingleOrDefault<MyMessageDetailsModel>(sql, parameters);
            }
        }

        public PagedList<MyMessageModel> Get(Guid userId, GetMyMessagesQueryParams queryParams)
        {
            var sql =
                @"select 
                    [id]			as [Id],
	                [subject]		as [Subject],
                    [created_date]	as [CreatedDate]
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
                offset = (queryParams.PageNumber - 1) * queryParams.PageSize,
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

                    return new PagedList<MyMessageModel>(items, totalCount, queryParams.PageNumber, queryParams.PageSize);
                }
            }
        }
    }
}
