using Core.Application.Helpers;
using System;

namespace Core.Application.Messages.Queries.GetMyMessages
{
    public sealed class GetMyMessagesQuery : IGetMyMessagesQuery
    {
        private readonly IMessageReadOnlyRepository _repository;
        public GetMyMessagesQuery(IMessageReadOnlyRepository repository)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        public PagedList<MyMessageModel> Execute(Guid userId, GetMyMessagesQueryParams queryParams)
        {
            if (queryParams == null)
                return PagedList<MyMessageModel>.CreateEmpty();

            return _repository.Get(userId, queryParams);
        }
    }
}
