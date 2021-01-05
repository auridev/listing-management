using Core.Domain.ValueObjects;
using System;

namespace Core.Domain.Profiles
{
    public sealed class PassiveProfile : Profile
    {
        public DateTimeOffset DeactivationDate { get; }
        public TrimmedString Reason { get; }

        private PassiveProfile() { }

        public PassiveProfile(Guid id,
            Guid userId,
            Email email,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            UserPreferences userPreferences,
            DateTimeOffset createdDate,
            DateTimeOffset deactivationDate,
            TrimmedString reason)
            : base(id, userId, email, contactDetails, locationDetails, geographicLocation, userPreferences, createdDate)
        {
            if (deactivationDate == default)
                throw new ArgumentNullException(nameof(deactivationDate));
            if (reason == null)
                throw new ArgumentNullException(nameof(reason));

            DeactivationDate = deactivationDate;
            Reason = reason;
        }
    }
}
