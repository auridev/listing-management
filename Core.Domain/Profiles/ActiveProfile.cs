using Core.Domain.ValueObjects;
using LanguageExt;
using System;

namespace Core.Domain.Profiles
{
    public sealed class ActiveProfile : Profile
    {
        // because of ORM limitations
        public SeenDate ___efCoreSeenDate { get; private set; }
        public Option<SeenDate> IntroductionSeenOn
        {
            get
            {
                return ___efCoreSeenDate == null ? Option<SeenDate>.None : ___efCoreSeenDate;
            }
            private set
            {
                value
                    .Some(v => { 
                        ___efCoreSeenDate = v; 
                    })
                    .None(() => { 
                        ___efCoreSeenDate = null; 
                    });
            }
        }

        private ActiveProfile() { }

        public ActiveProfile(Guid id,
            Guid userId,
            Email email,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            UserPreferences userPreferences,
            DateTimeOffset createdDate,
            Option<SeenDate> introductionSeenOn)
            : base(id, userId, email, contactDetails, locationDetails, geographicLocation, userPreferences, createdDate)
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
                CreatedDate,
                deactivationDate,
                reason);
        }

        public void HasSeenIntroduction(SeenDate seenDate)
        {
            IntroductionSeenOn = seenDate;
        }
    }
}
