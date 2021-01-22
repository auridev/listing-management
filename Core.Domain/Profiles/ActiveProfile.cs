using Common.Helpers;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using static Common.Helpers.Result;
using static LanguageExt.Prelude;

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
                    .Some(v =>
                    {
                        ___efCoreSeenDate = v;
                    })
                    .None(() =>
                    {
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
            DateTimeOffset createdDate)
            : base(id, userId, email, contactDetails, locationDetails, geographicLocation, userPreferences, createdDate)
        {
            IntroductionSeenOn = Option<SeenDate>.None;
        }

        public Either<Error, Unit> UpdateDetails(
            ContactDetails newContactDetails,
            LocationDetails newLocationDetails,
            GeographicLocation newGeographicLocation,
            UserPreferences newUserPreferences)
        {
            if (newContactDetails == null)
                return Invalid<Unit>(nameof(newContactDetails));
            if (newLocationDetails == null)
                return Invalid<Unit>(nameof(newLocationDetails));
            if (newGeographicLocation == null)
                return Invalid<Unit>(nameof(newGeographicLocation));
            if (newUserPreferences == null)
                return Invalid<Unit>(nameof(newUserPreferences));

            ContactDetails = newContactDetails;
            LocationDetails = newLocationDetails;
            GeographicLocation = newGeographicLocation;
            UserPreferences = newUserPreferences;

            return Success(unit);
        }

        public Either<Error, PassiveProfile> Deactivate(DateTimeOffset deactivationDate, TrimmedString reason)
        {
            if (deactivationDate == default)
                return Invalid<PassiveProfile>(nameof(deactivationDate));
            if (reason == null)
                return Invalid<PassiveProfile>(nameof(reason));

            return Success(new PassiveProfile(Id,
                UserId,
                Email,
                ContactDetails,
                LocationDetails,
                GeographicLocation,
                UserPreferences,
                CreatedDate,
                deactivationDate,
                reason));
        }

        public Either<Error, Unit> HasSeenIntroduction(SeenDate seenDate)
        {
            if (seenDate == null)
                return Invalid<Unit>(nameof(seenDate));

            IntroductionSeenOn = seenDate;

            return Success(unit);
        }
    }
}
