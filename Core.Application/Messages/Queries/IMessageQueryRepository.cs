using Core.Application.Helpers;
using Core.Application.Messages.Queries.GetMyMessageDetails;
using Core.Application.Messages.Queries.GetMyMessages;
using LanguageExt;
using System;

namespace Core.Application.Messages.Queries
{
    public interface IMessageQueryRepository
    {
        Option<MyMessageDetailsModel> Find(Guid userId, GetMyMessageDetailsQueryParams queryParams);
        PagedList<MyMessageModel> Get(Guid userId, GetMyMessagesQueryParams queryParams);
    }
}
