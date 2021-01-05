using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.Domain.Profiles
{
    public abstract class Profile : IEquatable<Profile>
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public Email Email { get; }
        public ContactDetails ContactDetails { get; protected set; }
        public LocationDetails LocationDetails { get; protected set; }
        public GeographicLocation GeographicLocation { get; protected set; }
        public UserPreferences UserPreferences { get; protected set; }
        public DateTimeOffset CreatedDate { get; }

        protected Profile()
        {
        }

        public Profile(Guid id,
            Guid userId,
            Email email,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            UserPreferences userPreferences,
            DateTimeOffset createdDate)
        {
            if (id == default)
                throw new ArgumentNullException(nameof(id));
            if (userId == default)
                throw new ArgumentNullException(nameof(userId));
            if (email == null)
                throw new ArgumentNullException(nameof(email));
            if (contactDetails == null)
                throw new ArgumentNullException(nameof(contactDetails));
            if (locationDetails == null)
                throw new ArgumentNullException(nameof(locationDetails));
            if (geographicLocation == null)
                throw new ArgumentNullException(nameof(geographicLocation));
            if (userPreferences == null)
                throw new ArgumentNullException(nameof(userPreferences));
            if (createdDate == default)
                throw new ArgumentNullException(nameof(createdDate));

            Id = id;
            UserId = userId;
            Email = email;
            ContactDetails = contactDetails;
            LocationDetails = locationDetails;
            GeographicLocation = geographicLocation;
            UserPreferences = userPreferences;
            CreatedDate = createdDate;
        }

        public bool Equals([AllowNull] Profile other)
        {
            if (GetType() != other.GetType())
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (Id.IsDefault() || other.Id.IsDefault())
                return false;

            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Profile other))
                return false;

            return Equals(other);
        }

        public static bool operator ==(Profile a, Profile b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Profile a, Profile b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().ToString() + Id).GetHashCode();
        }
    }
}
