using BusinessLine.Common.Dates;
using BusinessLine.Core.Domain.Listings;
using LanguageExt;
using System;

namespace BusinessLine.Core.Application.Listings.Commands.ReactivatePassiveListing
{
    public sealed class ReactivatePassiveListingCommand : IReactivatePassiveListingCommand
    {
        private readonly IListingRepository _repository;
        private readonly IDateTimeService _dateTimeService;

        public ReactivatePassiveListingCommand(IListingRepository repository, IDateTimeService dateTimeService)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }
        public void Execute(ReactivatePassiveListingModel model)
        {
            // Pre-requisites
            DateTimeOffset expirationDate = _dateTimeService.GetFutureUtcDateTime(Listing.DaysUntilExpiration);
            Option<PassiveListing> optionalPassiveListing = _repository.FindPassive(model.ListingId);

            // Command
            optionalPassiveListing
                .Some(passive =>
                {
                    ActiveListing active = passive.Reactivate(expirationDate);

                    _repository.Delete(passive);

                    _repository.Add(active);

                    _repository.Save();
                })
                .None(() => { });
        }
    }
}
