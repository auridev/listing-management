﻿using Core.Domain.ValueObjects;
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
            UserPreferences userPreferences,
            DateTimeOffset createdDate)
        {

            return new ActiveProfile(id,
                userid,
                email,
                contactDetails,
                locationDetails,
                geographicLocation,
                userPreferences,
                createdDate,
                Option<SeenDate>.None);
        }
    }
}
