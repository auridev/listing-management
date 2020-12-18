using LanguageExt;

namespace Core.Application.Profiles.Queries.GetPassiveProfileDetails
{
    public interface IGetPassiveProfileDetailsQuery
    {
        Option<PassiveProfileDetailsModel> Execute(GetPassiveProfileDetailsQueryParams queryParams);
    }
}