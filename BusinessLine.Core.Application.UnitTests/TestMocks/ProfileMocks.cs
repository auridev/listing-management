using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Profiles;
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
            SeenDate.CreateNone());
    }
}
