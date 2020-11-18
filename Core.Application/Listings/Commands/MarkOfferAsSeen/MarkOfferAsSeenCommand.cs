using Core.Domain.Common;
using Core.Domain.Offers;
using Common.Dates;
using LanguageExt;
using System;

namespace Core.Application.Listings.Commands.MarkOfferAsSeen
{
    public sealed class MarkOfferAsSeenCommand : IMarkOfferAsSeenCommand
    {
        private readonly IOfferRepository _repository;
        private readonly IDateTimeService _dateTimeService;

        public MarkOfferAsSeenCommand(IOfferRepository repository, IDateTimeService dateTimeService)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public void Execute(MarkOfferAsSeenModel model)
        {
            // Pre-requisites
            DateTimeOffset dateTimeOffset = _dateTimeService.GetCurrentUtcDateTime();
            SeenDate seenDate = SeenDate.Create(dateTimeOffset);
            Option<ReceivedOffer> optionalOffer = _repository.Find(model.OfferId);

            // Command
            optionalOffer
                .Some(offer =>
                {
                    offer.HasBeenSeen(seenDate);

                    _repository.Save();
                })
                .None(() => { });
        }
    }
}
