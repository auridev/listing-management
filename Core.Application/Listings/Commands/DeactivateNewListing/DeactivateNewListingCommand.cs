using Core.Domain.ValueObjects;
using Core.Domain.Listings;
using Common.Dates;
using LanguageExt;
using System;

namespace Core.Application.Listings.Commands.DeactivateNewListing
{
    public sealed class DeactivateNewListingCommand : IDeactivateNewListingCommand
    {
        private readonly IListingRepository _repository;
        private readonly IDateTimeService _dateTimeService;

        public DeactivateNewListingCommand(IListingRepository repository, IDateTimeService dateTimeService)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public void Execute(DeactivateNewListingModel model)
        {
            //// Pre-requisites
            //TrimmedString reason = TrimmedString.Create(model.Reason);
            //DateTimeOffset deactivationDate = _dateTimeService.GetCurrentUtcDateTime();
            //Option<NewListing> optionalNewListing = _repository.FindNew(model.ListingId);

            //// Command
            //optionalNewListing
            //    .Some(newListing =>
            //    {
            //        PassiveListing passiveListing = newListing.Deactivate(reason, deactivationDate);

            //        _repository.Delete(newListing);

            //        _repository.Add(passiveListing);

            //        _repository.Save();
            //    })
            //    .None(() => { });
        }
    }
}
