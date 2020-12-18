using LanguageExt;

namespace Core.Application.Profiles.Queries.GetUserProfileDetails
{
    public interface IGetUserProfileDetailsQuery
    {
        Option<UserProfileDetailsModel> Execute(GetUserProfileDetailsQueryParams queryParams);
    }
}