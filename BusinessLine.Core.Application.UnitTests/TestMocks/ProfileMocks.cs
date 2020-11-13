using Core.Domain.Common;
using Core.Domain.Profiles;
using LanguageExt;
using System;

namespace BusinessLine.Core.Application.UnitTests.TestMocks
{
    internal class ProfileMocks
    {
        public static ActiveProfile UK_Profile => new ActiveProfile(
            Guid.NewGuid(),
            Guid.NewGuid(),
            ValueObjectMocks.Email,
            ValueObjectMocks.ContactDetails,
            ValueObjectMocks.LocationDetails,
            ValueObjectMocks.GeographicLocation,
            ValueObjectMocks.UserPreferences,
            Option<SeenDate>.None);
    }
}
