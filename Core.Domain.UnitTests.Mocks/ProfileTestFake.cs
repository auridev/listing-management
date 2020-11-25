using Core.Domain.Common;
using Core.Domain.Profiles;
using System;

namespace Core.UnitTests.Mocks
{
    public class ProfileTestFake : Profile
    {
        public ProfileTestFake(Guid id,
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
