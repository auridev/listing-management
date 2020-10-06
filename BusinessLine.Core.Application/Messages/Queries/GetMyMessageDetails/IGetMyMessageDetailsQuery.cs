using LanguageExt;
using System;

namespace BusinessLine.Core.Application.Messages.Queries.GetMyMessageDetails
{
    public interface IGetMyMessageDetailsQuery
    {
        Option<MyMessageDetailsModel> Execute(Guid userId, GetMyMessageDetailsQueryParams queryParams);
    }
}