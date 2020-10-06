using LanguageExt;

namespace BusinessLine.Core.Application.Profiles.Queries.GetProfileDetails
{
    public interface IGetProfileDetailsQuery
    {
        Option<ProfileDetailsModel> Execute(GetProfileDetailsQueryParams queryParams);
    }
}