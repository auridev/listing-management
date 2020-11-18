using Core.Application.Listings.Commands.ReceiveOffer.Factory;
using Core.Domain.Common;
using Core.Domain.Listings;
using Core.Domain.Offers;
using LanguageExt;
using System;

namespace Core.Application.Listings.Commands.ReceiveOffer
{
    public sealed class ReceiveOfferCommand : IReceiveOfferCommand
    {
        private readonly IListingRepository _listingRepository;
        private readonly IOfferFactory _offerFactory;

        public ReceiveOfferCommand(IListingRepository listingRepository, IOfferFactory offerFactory)
        {
            _listingRepository = listingRepository ??
                throw new ArgumentNullException(nameof(listingRepository));
            _offerFactory = offerFactory ??
                throw new ArgumentNullException(nameof(offerFactory));
        }
        public void Execute(Guid userId, ReceiveOfferModel model)
        {
            // Pre-requisites
            Option<ActiveListing> optionalActiveListing =
                _listingRepository.FindActive(model.ListingId);
            ReceivedOffer offer = _offerFactory.Create(
                Owner.Create(userId),
                MonetaryValue.Create(
                    model.Value,
                    CurrencyCode.Create(model.CurrencyCode)));

            // Command
            optionalActiveListing
                .Some(activeListing =>
                {
                    activeListing.ReceiveOffer(offer);

                    _listingRepository.Save();
                })
                .None(() => { });
        }
    }
}
