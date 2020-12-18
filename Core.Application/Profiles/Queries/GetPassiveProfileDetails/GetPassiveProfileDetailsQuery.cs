using LanguageExt;

namespace Core.Application.Profiles.Queries.GetPassiveProfileDetails
{
    public class GetPassiveProfileDetailsQuery : IGetPassiveProfileDetailsQuery
    {
        private readonly IProfileReadOnlyRepository _repository;

        public GetPassiveProfileDetailsQuery(IProfileReadOnlyRepository repository)
        {
            _repository = repository;
        }

        public Option<PassiveProfileDetailsModel> Execute(GetPassiveProfileDetailsQueryParams queryParams)
        {
            if (queryParams == null)
                return Option<PassiveProfileDetailsModel>.None;

            return _repository.FindPassiveProfile(queryParams.ProfileId);
        }
    }
}
