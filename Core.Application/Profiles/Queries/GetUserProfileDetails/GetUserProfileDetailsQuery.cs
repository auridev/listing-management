using LanguageExt;

namespace Core.Application.Profiles.Queries.GetUserProfileDetails
{
    public class GetUserProfileDetailsQuery : IGetUserProfileDetailsQuery
    {
        private readonly IProfileReadOnlyRepository _repository;

        public GetUserProfileDetailsQuery(IProfileReadOnlyRepository repository)
        {
            _repository = repository;
        }

        public Option<UserProfileDetailsModel> Execute(GetUserProfileDetailsQueryParams queryParams)
        {
            if (queryParams == null)
                return Option<UserProfileDetailsModel>.None;

            return _repository.FindUserProfile(queryParams.UserId);
        }
    }
}
