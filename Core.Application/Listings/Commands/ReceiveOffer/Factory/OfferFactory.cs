using Common.Dates;
using Common.Helpers;
using Core.Domain.Offers;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using static Common.Helpers.Result;

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

        public Either<Error, ReceivedOffer> Create(Owner owner, MonetaryValue monetaryValue)
        {
            if (owner == null)
                return Invalid<ReceivedOffer>(nameof(owner));
            if (monetaryValue == null)
                return Invalid<ReceivedOffer>(nameof(monetaryValue));

            DateTimeOffset nowInUtc =
                _dateTimeService.GetCurrentUtcDateTime();

            var receivedOffer = new ReceivedOffer(Guid.NewGuid(), owner, monetaryValue, nowInUtc);

            return Success(receivedOffer);
        }
    }
}
