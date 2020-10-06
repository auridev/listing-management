using BusinessLine.Common.Dates;
using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Listings;
using LanguageExt;
using System;

namespace BusinessLine.Core.Application.Listings.Commands.ReceiveOffer.Factory
{
    public sealed class OfferFactory : IOfferFactory
    {
        private readonly IDateTimeService _dateTimeService;

        public OfferFactory(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public Offer Create(Owner owner, MonetaryValue monetaryValue)
        {
            DateTimeOffset nowInUtc = 
                _dateTimeService.GetCurrentUtcDateTime();

            return new Offer(Guid.NewGuid(), owner, monetaryValue, nowInUtc, SeenDate.CreateNone());
        }
    }
}
