using BusinessLine.Core.Domain.Common;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace BusinessLine.Core.Domain.Listings
{
    public sealed class ActiveListing : Listing
    {
        public DateTimeOffset ExpirationDate { get; }
        public ICollection<Offer> Offers { get; } = new List<Offer>();

        public ActiveListing(Guid id,
            Owner owner,
            ListingDetails listingDetails,
            ContactDetails contactDetails,
            LocationDetails locationDetails,
            GeographicLocation geographicLocation,
            DateTimeOffset expirationDate) 
            : base(id, owner, listingDetails, contactDetails, locationDetails, geographicLocation)
        {
            if (expirationDate == default)
                throw new ArgumentNullException(nameof(expirationDate));

            ExpirationDate = expirationDate;
        }

        public PassiveListing Deactivate(TrimmedString trimmedString, DateTimeOffset deactivationDate)
        {
            return new PassiveListing(Id,
                Owner,
                ListingDetails,
                ContactDetails,
                LocationDetails,
                GeographicLocation,
                deactivationDate,
                trimmedString);
        }

        public void ReceiveOffer(Offer offer)
        {
            if (Owner == offer.Owner) // ignore offers from owner of the listing 
                return;

            Offers // remove existing offers from the same owner
                .Find(o => o.Owner == offer.Owner)
                .IfSome(o => Offers.Remove(o));

            Offers.Add(offer);
        }

        public Option<ClosedListing> AcceptOffer(Offer offer, DateTimeOffset closedOn)
        {
            var possibleListing = Offers
                .Find(o => o == offer)
                .Match(
                    // offer found in received offers => successfully close the listing
                    some => new ClosedListing(Id,
                        Owner,
                        ListingDetails,
                        ContactDetails,
                        LocationDetails,
                        GeographicLocation,
                        closedOn,
                        some),
                    // offer NOT found in received offers => don't close 
                    () => Option<ClosedListing>.None);

            return possibleListing;
        }
    }
}
