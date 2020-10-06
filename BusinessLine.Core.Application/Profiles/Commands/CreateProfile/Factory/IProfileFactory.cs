using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Profiles;
using System;

namespace BusinessLine.Core.Application.Profiles.Commands.CreateProfile.Factory
{
    public interface IProfileFactory
    {
        ActiveProfile CreateActive(Guid id, 
            Guid userid, 
            Email email, 
            ContactDetails contactDetails, 
            LocationDetails locationDetails, 
            GeographicLocation geographicLocation, 
            UserPreferences userPreferences);
    }
}