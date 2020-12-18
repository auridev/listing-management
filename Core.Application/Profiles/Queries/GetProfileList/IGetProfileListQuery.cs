using Core.Application.Helpers;
using System;

namespace Core.Application.Profiles.Queries.GetProfileList
{
    public interface IGetProfileListQuery
    {
        PagedList<ProfileModel> Execute(GetProfileListQueryParams queryParams);
    }
}