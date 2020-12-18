using LanguageExt;
using System;

namespace Core.Application.Profiles.Queries.GetActiveProfileDetails
{
    public sealed class GetActiveProfileDetailsQuery : IGetActiveProfileDetailsQuery
    {
        private readonly IProfileReadOnlyRepository _repository;

        public GetActiveProfileDetailsQuery(IProfileReadOnlyRepository repository)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        public Option<ActiveProfileDetailsModel> Execute(GetActiveProfileDetailsQueryParams queryParams)
        {
            if (queryParams == null)
                return Option<ActiveProfileDetailsModel>.None;

            return _repository.FindActiveProfile(queryParams.ProfileId);
        }
    }
}
