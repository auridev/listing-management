using Core.Application.Helpers;
using System;

namespace Core.Application.Profiles.Queries.GetProfileList
{
    public sealed class GetProfileListQuery : IGetProfileListQuery
    {
        private readonly IProfileReadOnlyRepository _repository;

        public GetProfileListQuery(IProfileReadOnlyRepository repository)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        public PagedList<ProfileModel> Execute(GetProfileListQueryParams queryParams)
        {
            if (queryParams == null)
                return PagedList<ProfileModel>.CreateEmpty();

            return _repository.Get(queryParams);
        }
    }
}
