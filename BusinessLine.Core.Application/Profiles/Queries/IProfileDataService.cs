using BusinessLine.Core.Application.Profiles.Queries.GetProfileDetails;
using BusinessLine.Core.Application.Profiles.Queries.GetProfileList;
using System;
using System.Collections.Generic;

namespace BusinessLine.Core.Application.Profiles.Queries
{
    public interface IProfileDataService
    {
        ProfileDetailsModel Find(Guid id);
        ICollection<ProfileModel> Get(Guid userId, GetProfileListQueryParams queryParams);
    }
}
