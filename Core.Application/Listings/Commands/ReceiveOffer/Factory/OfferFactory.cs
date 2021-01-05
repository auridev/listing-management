using Core.Domain.ValueObjects;
using Core.Domain.Offers;
using Common.Dates;
using LanguageExt;
using System;

namespace Core.Application.Listings.Commands.ReceiveOffer.Factory
{
    public sealed class OfferFactory : IOfferFactory
    {
        private readonly IDateTimeService _dateTimeService;

        public OfferFactory(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public ReceivedOffer Create(Owner owner, MonetaryValue monetaryValue)
        {
            DateTimeOffset nowInUtc =
                _dateTimeService.GetCurrentUtcDateTime();

            return new ReceivedOffer(Guid.NewGuid(), owner, monetaryValue, nowInUtc, Option<SeenDate>.None);
        }
    }
}
