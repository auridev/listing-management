using BusinessLine.Common.Dates;
using BusinessLine.Core.Domain.Listings;
using LanguageExt;
using System;

namespace BusinessLine.Core.Application.Listings.Commands.ActivateSuspiciousListing
{
    public sealed class ActivateSuspiciousListingCommand : IActivateSuspiciousListingCommand
    {
        private readonly IListingRepository _repository;
        private readonly IDateTimeService _dateTimeService;

        public ActivateSuspiciousListingCommand(IListingRepository repository, IDateTimeService dateTimeService)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public void Execute(ActivateSuspiciousListingModel model)
        {
            // Pre-requisites
            DateTimeOffset expirationDate = _dateTimeService.GetFutureUtcDateTime(Listing.DaysUntilExpiration);
            Option<SuspiciousListing> optionalSuspiciousListing = _repository.FindSuspicious(model.ListingId);

            // Command
            optionalSuspiciousListing
                .Some(suspiciousListing =>
                {
                    ActiveListing activeListing = suspiciousListing.Activate(expirationDate);

                    _repository.Delete(suspiciousListing);

                    _repository.Add(activeListing);

                    _repository.Save();
                })
                .None(() => { });
        }
    }
}
