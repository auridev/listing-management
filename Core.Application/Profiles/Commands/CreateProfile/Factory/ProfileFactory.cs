using Common.Dates;
using Common.Helpers;
using Core.Domain.Profiles;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using static Common.Helpers.Result;

namespace Core.Application.Profiles.Commands.CreateProfile.Factory
{
    public class ProfileFactory : IProfileFactory
    {
        private readonly IDateTimeService _dateTimeService;

        public ProfileFactory(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public Either<Error, ActiveProfile> CreateActive(
            Guid userId,
            Email email,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            UserPreferences userPreferences)
        {
            if (userId == default)
                return Invalid<ActiveProfile>(nameof(userId));
            if (email == null)
                return Invalid<ActiveProfile>(nameof(email));
            if (contactDetails == null)
                return Invalid<ActiveProfile>(nameof(contactDetails));
            if (locationDetails == null)
                return Invalid<ActiveProfile>(nameof(locationDetails));
            if (geographicLocation == null)
                return Invalid<ActiveProfile>(nameof(geographicLocation));
            if (userPreferences == null)
                return Invalid<ActiveProfile>(nameof(userPreferences));

            var activeProfile = new ActiveProfile(
                Guid.NewGuid(),
                userId,
                email,
                contactDetails,
                locationDetails,
                geographicLocation,
                userPreferences,
                _dateTimeService.GetCurrentUtcDateTime());

            return Success(activeProfile);
        }
    }
}
