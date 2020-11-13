using System;
using System.Collections.Generic;

namespace Core.Application.Profiles.Queries.GetProfileList
{
    public interface IGetProfileListQuery
    {
        ICollection<ProfileModel> Execute(Guid userId, GetProfileListQueryParams queryParams);
    }
}