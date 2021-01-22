using Core.Application.Helpers;
using Core.Application.Messages.Queries.GetMyMessageDetails;
using Core.Application.Messages.Queries.GetMyMessages;
using LanguageExt;
using System;

namespace Core.Application.Messages.Queries
{
    public interface IMessageReadOnlyRepository
    {
        Option<MyMessageDetailsModel> Find(Guid userId, Guid messageId);
        PagedList<MyMessageModel> Get(Guid userId, GetMyMessagesQueryParams queryParams);
    }
}
