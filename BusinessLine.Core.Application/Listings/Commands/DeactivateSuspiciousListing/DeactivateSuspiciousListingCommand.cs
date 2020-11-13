using Core.Domain.Common;
using Core.Domain.Listings;
using Common.Dates;
using LanguageExt;
using System;

namespace Core.Application.Listings.Commands.DeactivateSuspiciousListing
{
    public sealed class DeactivateSuspiciousListingCommand : IDeactivateSuspiciousListingCommand
    {
        private readonly IListingRepository _repository;
        private readonly IDateTimeService _dateTimeService;

        public DeactivateSuspiciousListingCommand(IListingRepository repository, IDateTimeService dateTimeService)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public void Execute(DeactivateSuspiciousListingModel model)
        {
            // Pre-requisites
            TrimmedString reason = TrimmedString.Create(model.Reason);
            DateTimeOffset deactivationDate = _dateTimeService.GetCurrentUtcDateTime();
            Option<SuspiciousListing> optionalSuspiciousListing = _repository.FindSuspicious(model.ListingId);

            // Command
            optionalSuspiciousListing
                .Some(suspiciousListing =>
                {
                    PassiveListing passiveListing = suspiciousListing.Deactivate(reason, deactivationDate);

                    _repository.Delete(suspiciousListing);

                    _repository.Add(passiveListing);

                    _repository.Save();
                })
                .None(() => { });
        }
    }
}
