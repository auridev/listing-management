using BusinessLine.Common.Dates;
using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Listings;
using LanguageExt;
using System;

namespace BusinessLine.Core.Application.Listings.Commands.AddLead
{
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
            Owner owner =
                Owner.Create(userId);
            Option<ActiveListing> optionalActiveListing =
                _repository.FindActive(model.ListingId);

            // Command
            optionalActiveListing
                .Some(listing =>
                {
                    var lead = Lead.Create(owner, createdDate);

                    _repository.Add(lead);

                    _repository.Save();
                })
                .None(() => { });
        }
    }
}
