using Common.Dates;
using Core.Domain.Common;
using Core.Domain.Listings;
using LanguageExt;
using System;

namespace Core.Application.Listings.Commands.MarkOfferAsSeen
{
    public sealed class MarkOfferAsSeenCommand : IMarkOfferAsSeenCommand
    {
        private readonly IListingRepository _listingRepository;
        private readonly IDateTimeService _dateTimeService;

        public MarkOfferAsSeenCommand(IListingRepository listingRepository, IDateTimeService dateTimeService)
        {
            _listingRepository = listingRepository ??
                throw new ArgumentNullException(nameof(listingRepository));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public void Execute(MarkOfferAsSeenModel model)
        {
            // Pre-requisites
            DateTimeOffset dateTimeOffset = 
                _dateTimeService.GetCurrentUtcDateTime();
            SeenDate seenDate = 
                SeenDate.Create(dateTimeOffset);
            Option<ActiveListing> optionalActiveListing =
                _listingRepository.FindActive(model.ListingId);

            // Command
            optionalActiveListing
                .IfSome(l =>
                {
                    l.MarkOfferAsSeen(model.OfferId, seenDate);

                    _listingRepository.Update(l);

                    _listingRepository.Save();
                });
        }
    }
}
