using Common.Helpers;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using static Common.Helpers.Result;
using static LanguageExt.Prelude;

namespace Core.Domain.Listings
{
    public sealed class NewListing : Listing
    {
        private NewListing() { }

        public NewListing(Guid id,
            Owner owner,
            ListingDetails listingDetails,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            DateTimeOffset createdDate)
            : base(id, owner, listingDetails, contactDetails, locationDetails, geographicLocation, createdDate)
        {
            if (createdDate == default)
                throw new ArgumentNullException(nameof(createdDate));
        }

        public Either<Error, PassiveListing> Deactivate(TrimmedString reason, DateTimeOffset deactivationDate)
        {
            if (reason == null)
                return Invalid<PassiveListing>(nameof(reason));
            if (deactivationDate == default)
                return Invalid<PassiveListing>(nameof(deactivationDate));

            var passiveListing = new PassiveListing(Id,
                Owner,
                ListingDetails,
                ContactDetails,
                LocationDetails,
                GeographicLocation,
                CreatedDate,
                deactivationDate,
                reason);

            return Success(passiveListing);
        }

        public Either<Error, ActiveListing> Activate(DateTimeOffset expirationDate)
        {
            if (expirationDate == default)
                return Invalid<ActiveListing>(nameof(expirationDate));

            var activeListing = new ActiveListing(
                Id,
                Owner,
                ListingDetails,
                ContactDetails,
                LocationDetails,
                GeographicLocation,
                CreatedDate,
                expirationDate);

            return Success(activeListing);
        }

        public Either<Error, SuspiciousListing> MarkAsSuspicious(DateTimeOffset markedAsSuspiciousAt, TrimmedString reason)
        {
            if (markedAsSuspiciousAt == default)
                return Invalid<SuspiciousListing>(nameof(markedAsSuspiciousAt));
            if (reason == null)
                return Invalid<SuspiciousListing>(nameof(reason));

            var suspiciousListing = new SuspiciousListing(
                Id,
                Owner,
                ListingDetails,
                ContactDetails,
                LocationDetails,
                GeographicLocation,
                CreatedDate,
                markedAsSuspiciousAt,
                reason);

            return Success(suspiciousListing);
        }
    }
}
