using Core.Domain.Common;
using Core.Domain.Profiles;
using System;

namespace BusinessLine.Core.Domain.UnitTests.TestMocks
{
    // used only to test abstract profile stuff
    internal class ProfileMock : Profile
    {
        public ProfileMock(Guid id,
            Guid userId,
            Email email,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            UserPreferences userPreferences)
            : base(id, userId, email, contactDetails, locationDetails, geographicLocation, userPreferences)
        {
        }
    }
}
