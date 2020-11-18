using Core.Domain.Common;
using Core.Domain.Listings;
using Common.Dates;
using LanguageExt;
using System;

namespace Core.Application.Listings.Commands.DeactivateActiveListing
{
    public sealed class DeactivateActiveListingCommand : IDeactivateActiveListingCommand
    {
        private readonly IListingRepository _repository;
        private readonly IDateTimeService _dateTimeService;

        public DeactivateActiveListingCommand(IListingRepository repository, IDateTimeService dateTimeService)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public void Execute(DeactivateActiveListingModel model)
        {
            // Pre-requisites
            TrimmedString reason = TrimmedString.Create(model.Reason);
            DateTimeOffset deactivationDate = _dateTimeService.GetCurrentUtcDateTime();
            Option<ActiveListing> optionalActiveListing = _repository.FindActive(model.ListingId);

            // Command
            optionalActiveListing
                .Some(activeListing =>
                {
                    PassiveListing passiveListing = activeListing.Deactivate(reason, deactivationDate);

                    _repository.Delete(activeListing);

                    _repository.Add(passiveListing);

                    _repository.Save();
                })
                .None(() => { });
        }
    }
}
