using Core.Domain.Common;
using Core.Domain.Listings;
using Common.Dates;
using LanguageExt;
using System;

namespace Core.Application.Listings.Commands.MarkNewListingAsSuspicious
{
    public sealed class MarkNewListingAsSuspiciousCommand : IMarkNewListingAsSuspiciousCommand
    {
        private readonly IListingRepository _repository;
        private readonly IDateTimeService _dateTimeService;

        public MarkNewListingAsSuspiciousCommand(IListingRepository repository, IDateTimeService dateTimeService)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public void Execute(MarkNewListingAsSuspiciousModel model)
        {
            // Pre-requisites
            TrimmedString reason = TrimmedString.Create(model.Reason);
            DateTimeOffset markedAtDate = _dateTimeService.GetCurrentUtcDateTime();
            Option<NewListing> optionalNewListing = _repository.FindNew(model.ListingId);

            // Command
            optionalNewListing
                .Some(newListing =>
                {
                    SuspiciousListing suspiciousListing = newListing.MarkAsSuspicious(markedAtDate, reason);

                    _repository.Delete(newListing);

                    _repository.Add(suspiciousListing);

                    _repository.Save();
                })
                .None(() => { });
        }
    }
}
