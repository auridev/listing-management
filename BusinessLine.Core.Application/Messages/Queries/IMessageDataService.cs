using Core.Application.Messages.Queries.GetMyMessageDetails;
using Core.Application.Messages.Queries.GetMyMessages;
using LanguageExt;
using System;
using System.Collections.Generic;

namespace Core.Application.Messages.Queries
{
    public interface IMessageDataService
    {
        Option<MyMessageDetailsModel> Find(Guid userId, GetMyMessageDetailsQueryParams queryParams);
        ICollection<MyMessageModel> Get(Guid userId, GetMyMessagesQueryParams queryParams);
    }
}
