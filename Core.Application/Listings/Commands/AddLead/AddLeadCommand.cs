using Core.Domain.Common;
using Core.Domain.Listings;
using Common.Dates;
using LanguageExt;
using System;

namespace Core.Application.Listings.Commands.AddLead
{
    //marks an active listing seen by a user
    public sealed class AddLeadCommand : IAddLeadCommand
    {
        private readonly IListingRepository _repository;
        private readonly IDateTimeService _service;

        public AddLeadCommand(IListingRepository repository, IDateTimeService service)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            _service = service ??
                throw new ArgumentNullException(nameof(service));
        }

        public void Execute(Guid userId, AddLeadModel model)
        {
            // Pre-requisites
            DateTimeOffset createdDate =
                _service.GetCurrentUtcDateTime();
            Owner userInterested =
                Owner.Create(userId);
            Option<ActiveListing> optionalActiveListing =
                _repository.FindActive(model.ListingId);

            // Command
            optionalActiveListing
                .Some(listing =>
                {
                    var lead = Lead.Create(userInterested, createdDate);

                    listing.AddLead(lead);

                    _repository.Save();
                })
                .None(() => { });
        }
    }
}
