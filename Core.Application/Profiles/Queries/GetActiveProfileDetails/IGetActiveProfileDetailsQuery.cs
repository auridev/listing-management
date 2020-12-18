using LanguageExt;

namespace Core.Application.Profiles.Queries.GetActiveProfileDetails
{
    public interface IGetActiveProfileDetailsQuery
    {
        Option<ActiveProfileDetailsModel> Execute(GetActiveProfileDetailsQueryParams queryParams);
    }
}