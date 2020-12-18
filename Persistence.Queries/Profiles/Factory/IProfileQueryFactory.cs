using Core.Application.Profiles.Queries.GetProfileList;
using Dapper;

namespace Persistence.Queries.Profiles.Factory
{
    public interface IProfileQueryFactory
    {
        string CreateText(GetProfileListQueryParams queryParams);

        DynamicParameters CreateParameters(GetProfileListQueryParams queryParams);
    }
}