using System;
using System.Collections.Generic;

namespace Core.Application.Messages.Queries.GetMyMessages
{
    public sealed class GetMyMessagesQuery : IGetMyMessagesQuery
    {
        private readonly IMessageQueryRepository _dataService;
        public GetMyMessagesQuery(IMessageQueryRepository dataService)
        {
            _dataService = dataService ??
                throw new ArgumentNullException(nameof(dataService));
        }

        public ICollection<MyMessageModel> Execute(Guid userId, GetMyMessagesQueryParams queryParams)
        {
            if (queryParams == null)
                return new MyMessageModel[0];

            return _dataService.Get(userId, queryParams);
        }
    }
}
