using Core.Domain.Common;
using Core.Domain.Profiles;
using LanguageExt;
using System;

namespace Core.Application.Profiles.Commands.CreateProfile.Factory
{
    public class ProfileFactory : IProfileFactory
    {
        public ActiveProfile CreateActive(Guid id,
            Guid userid,
            Email email,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            UserPreferences userPreferences)
        {

            return new ActiveProfile(id,
                userid,
                email,
                contactDetails,
                locationDetails,
                geographicLocation,
                userPreferences,
                Option<SeenDate>.None);
        }
    }
}
