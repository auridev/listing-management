using System;
using System.Collections.Generic;

namespace Core.Application.Messages.Queries.GetMyMessages
{
    public interface IGetMyMessagesQuery
    {
        ICollection<MyMessageModel> Execute(Guid userId, GetMyMessagesQueryParams queryParams);
    }
}