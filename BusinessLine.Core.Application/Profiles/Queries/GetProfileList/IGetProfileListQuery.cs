using System;
using System.Collections.Generic;

namespace BusinessLine.Core.Application.Profiles.Queries.GetProfileList
{
    public interface IGetProfileListQuery
    {
        ICollection<ProfileModel> Execute(Guid userId, GetProfileListQueryParams queryParams);
    }
}