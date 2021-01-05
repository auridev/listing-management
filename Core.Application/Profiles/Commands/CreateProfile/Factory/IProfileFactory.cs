using Core.Domain.ValueObjects;
using Core.Domain.Profiles;
using System;

namespace Core.Application.Profiles.Commands.CreateProfile.Factory
{
    public interface IProfileFactory
    {
        ActiveProfile CreateActive(Guid id, 
            Guid userid, 
            Email email, 
            ContactDetails contactDetails, 
            LocationDetails locationDetails, 
            GeographicLocation geographicLocation, 
            UserPreferences userPreferences,
            DateTimeOffset createdDate);
    }
}