using Core.Application.Helpers;
using Core.Application.Profiles.Queries.GetActiveProfileDetails;
using Core.Application.Profiles.Queries.GetPassiveProfileDetails;
using Core.Application.Profiles.Queries.GetProfileList;
using Core.Application.Profiles.Queries.GetUserProfileDetails;
using LanguageExt;
using System;

namespace Core.Application.Profiles.Queries
{
    public interface IProfileReadOnlyRepository
    {
        Option<ActiveProfileDetailsModel> FindActiveProfile(Guid profileId);
        Option<PassiveProfileDetailsModel> FindPassiveProfile(Guid profileId);
        Option<UserProfileDetailsModel> FindUserProfile(Guid userId);
        PagedList<ProfileModel> Get(GetProfileListQueryParams queryParams);
    }
}
