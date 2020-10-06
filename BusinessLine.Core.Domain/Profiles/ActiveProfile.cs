using BusinessLine.Core.Domain.Common;
using System;

namespace BusinessLine.Core.Domain.Profiles
{
    public sealed class ActiveProfile : Profile
    {
        public SeenDate IntroductionSeenOn { get; private set; }

        private ActiveProfile() { }

        public ActiveProfile(Guid id,
            Guid userId,
            Email email,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            UserPreferences userPreferences,
            SeenDate introductionSeenOn)
            : base(id, userId, email, contactDetails, locationDetails, geographicLocation, userPreferences)
        {
            IntroductionSeenOn = introductionSeenOn;
        }

        public void UpdateDetails(ContactDetails newContactDetails,
            LocationDetails newLocationDetails,
            GeographicLocation newGeographicLocation,
            UserPreferences newUserPreferences)
        {
            ContactDetails = newContactDetails;
            LocationDetails = newLocationDetails;
            GeographicLocation = newGeographicLocation;
            UserPreferences = newUserPreferences;
        }

        public PassiveProfile Deactivate(DateTimeOffset deactivationDate, TrimmedString reason)
        {
            return new PassiveProfile(Id,
                UserId,
                Email,
                ContactDetails,
                LocationDetails,
                GeographicLocation,
                UserPreferences,
                deactivationDate,
                reason);
        }

        public void HasSeenIntroduction(SeenDate seenDate)
        {
            IntroductionSeenOn = seenDate;
        }
    }
}
