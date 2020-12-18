using Core.Application.Helpers;
using System;

namespace Core.Application.Messages.Queries.GetMyMessages
{
    public interface IGetMyMessagesQuery
    {
        PagedList<MyMessageModel> Execute(Guid userId, GetMyMessagesQueryParams queryParams);
    }
}