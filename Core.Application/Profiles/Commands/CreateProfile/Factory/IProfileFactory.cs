using Common.Helpers;
using Core.Domain.Profiles;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;

namespace Core.Application.Profiles.Commands.CreateProfile.Factory
{
    public interface IProfileFactory
    {
        Either<Error, ActiveProfile> CreateActive(
            Guid userid,
            Email email,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            UserPreferences userPreferences);
    }
}