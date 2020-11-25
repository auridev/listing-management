using Common.Dates;
using Core.Domain.Listings;
using LanguageExt;
using System;

namespace Core.Application.Listings.Commands.AcceptOffer
{
    public sealed class AcceptOfferCommand : IAcceptOfferCommand
    {
        private readonly IListingRepository _listingRepository;
        private readonly IDateTimeService _dateTimeService;

        public AcceptOfferCommand(IListingRepository listingRepository, IDateTimeService dateTimeService)
        {
            _listingRepository = listingRepository ??
                throw new ArgumentNullException(nameof(listingRepository));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public void Execute(AcceptOfferModel model)
        {
            Option<ActiveListing> optionalActiveListing =
                _listingRepository.FindActive(model.ListingId);
            DateTimeOffset closedOn =
                _dateTimeService.GetCurrentUtcDateTime();

            optionalActiveListing
                .IfSome(l =>
                {
                    Option<ClosedListing> optionalClosedListing = l.AcceptOffer(model.OfferId, closedOn);
                    optionalClosedListing
                        .IfSome(cl =>
                        {
                            _listingRepository.Add(cl);

                            _listingRepository.Delete(l);

                            _listingRepository.Save();

                        });
                });
        }
    }
}
