using BusinessLine.Common.Dates;
using BusinessLine.Core.Domain.Listings;
using LanguageExt;
using System;

namespace BusinessLine.Core.Application.Listings.Commands.AcceptOffer
{
    public sealed class AcceptOfferCommand : IAcceptOfferCommand
    {
        private readonly IListingRepository _listingRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly IDateTimeService _dateTimeService;

        public AcceptOfferCommand(IListingRepository listingRepository,
            IOfferRepository offerRepository,
            IDateTimeService dateTimeService)
        {
            _listingRepository = listingRepository ??
                throw new ArgumentNullException(nameof(listingRepository));
            _offerRepository = offerRepository ??
                throw new ArgumentNullException(nameof(offerRepository));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public void Execute(AcceptOfferModel model)
        {
            Option<ActiveListing> optionalActiveListing =
                _listingRepository.FindActive(model.ListingId);
            Option<Offer> optionalOffer =
                _offerRepository.Find(model.OfferId);
            DateTimeOffset closedOn =
                _dateTimeService.GetCurrentUtcDateTime();

            optionalActiveListing
                .Some(l =>
                {
                    optionalOffer
                        .Some(o =>
                        {
                            Option<ClosedListing> optionalClosedListing = l.AcceptOffer(o, closedOn);
                            optionalClosedListing
                                .Some(cl =>
                                {
                                    _listingRepository.Add(cl);

                                    _listingRepository.Delete(l);

                                    _listingRepository.Save();

                                })
                                .None(() => { });

                        })
                        .None(() => { });
                })
                .None(() => { });
        }
    }
}
