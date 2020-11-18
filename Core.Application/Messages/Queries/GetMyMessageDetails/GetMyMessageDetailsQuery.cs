using LanguageExt;
using System;

namespace Core.Application.Messages.Queries.GetMyMessageDetails
{
    public sealed class GetMyMessageDetailsQuery : IGetMyMessageDetailsQuery
    {
        private readonly IMessageDataService _dataService;
        public GetMyMessageDetailsQuery(IMessageDataService dataService)
        {
            _dataService = dataService ??
                throw new ArgumentNullException(nameof(dataService));
        }

        public Option<MyMessageDetailsModel> Execute(Guid userId, GetMyMessageDetailsQueryParams queryParams)
        {
            if (queryParams == null)
                return Option<MyMessageDetailsModel>.None;

            return _dataService.Find(userId, queryParams);
        }
    }
}
