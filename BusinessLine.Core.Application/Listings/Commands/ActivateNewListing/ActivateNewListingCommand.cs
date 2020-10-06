using BusinessLine.Common.Dates;
using BusinessLine.Core.Domain.Listings;
using LanguageExt;
using System;

namespace BusinessLine.Core.Application.Listings.Commands.ActivateNewListing
{
    public sealed class ActivateNewListingCommand : IActivateNewListingCommand
    {
        private readonly IListingRepository _repository;
        private readonly IDateTimeService _dateTimeService;

        public ActivateNewListingCommand(IListingRepository repository, IDateTimeService dateTimeService)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public void Execute(ActivateNewListingModel model)
        {
            // Pre-requisites
            DateTimeOffset expirationDate = _dateTimeService.GetFutureUtcDateTime(Listing.DaysUntilExpiration);
            Option<NewListing> optionalNewListing = _repository.FindNew(model.ListingId);

            // Command
            optionalNewListing
                .Some(newListing =>
                {
                    ActiveListing activeListing = newListing.Activate(expirationDate);

                    _repository.Delete(newListing);

                    _repository.Add(activeListing);

                    _repository.Save();
                })
                .None(() => { });
        }
    }
}
