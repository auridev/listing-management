using LanguageExt;
using System;

namespace Core.Application.Messages.Queries.GetMyMessageDetails
{
    public sealed class GetMyMessageDetailsQuery : IGetMyMessageDetailsQuery
    {
        private readonly IMessageReadOnlyRepository _repository;
        public GetMyMessageDetailsQuery(IMessageReadOnlyRepository repository)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        public Option<MyMessageDetailsModel> Execute(Guid userId, Guid messageId)
        {
            if ((userId == default) || (messageId == default))
                return Option<MyMessageDetailsModel>.None;

            return _repository.Find(userId, messageId);
        }
    }
}
