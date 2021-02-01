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
        private NewListing()
        {
        }

        public NewListing(Guid id,
            Owner owner,
            ListingDetails listingDetails,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            DateTimeOffset createdDate)
            : base(id, owner, listingDetails, contactDetails, locationDetails, geographicLocation, createdDate)
        {
        }

        public Either<Error, PassiveListing> MarkAsPassive(TrimmedString reason)
        {
            if (reason == null)
                return Invalid<PassiveListing>(nameof(reason));

            var passiveListing = new PassiveListing(Id,
                Owner,
                ListingDetails,
                ContactDetails,
                LocationDetails,
                GeographicLocation,
                CreatedDate,
                reason);

            return Success(passiveListing);
        }

        public Either<Error, ActiveListing> MarkAsActive(DateTimeOffset expirationDate)
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

        public Either<Error, SuspiciousListing> MarkAsSuspicious(TrimmedString reason)
        {
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
                reason);

            return Success(suspiciousListing);
        }
    }
}
