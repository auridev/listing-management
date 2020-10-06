using System;
using System.Collections.Generic;

namespace BusinessLine.Core.Application.Messages.Queries.GetMyMessages
{
    public interface IGetMyMessagesQuery
    {
        ICollection<MyMessageModel> Execute(Guid userId, GetMyMessagesQueryParams queryParams);
    }
}