using Common.Dates;
using Common.Helpers;
using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using static Common.Helpers.Functions;
using static LanguageExt.Prelude;

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

        public Either<Error, Unit> Execute(Guid userId, AddLeadModel model)
        {
            Either<Error, Guid> eitherUserId = EnsureNonDefault(userId);
            Either<Error, AddLeadModel> eitherModel = EnsureNotNull(model);
            Either<Error, ActiveListing> listing = FindActiveListing(eitherModel);
            Either<Error, Lead> lead =
                CreateLead(
                    userId,
                    _service.GetCurrentUtcDateTime());

            Either<Error, Unit> addLeadResult =
                AddLead(
                    listing,
                    lead);
            Either<Error, Unit> persistChangesResult =
                PersistChanges(
                    addLeadResult,
                    listing);

            return persistChangesResult;
        }

        private Either<Error, ActiveListing> FindActiveListing(Either<Error, AddLeadModel> eitherModel)
            =>
                eitherModel
                    .Map(model => _repository.FindActive(model.ListingId))
                    .Bind(option => option.ToEither<Error>(new Error.NotFound("active listing not found")));

        private Either<Error, Lead> CreateLead(Either<Error, Guid> eitherUserId, DateTimeOffset createdDate)
            =>
                eitherUserId
                    .Bind(userId => Lead.Create(userId, createdDate));

        private Either<Error, Unit> AddLead(Either<Error, ActiveListing> eitherActiveListing, Either<Error, Lead> eitherLead)
            =>
                (
                    from lead in eitherLead
                    from activeListing in eitherActiveListing
                    select (lead, activeListing)
                )
                .Bind(
                    context =>
                        context.activeListing.AddLead(context.lead));

        private Either<Error, Unit> PersistChanges(Either<Error, Unit> eitherAddLead, Either<Error, ActiveListing> eitherActiveListing)
            =>
                (
                    from addLeadSuccessful in eitherAddLead
                    from activeListing in eitherActiveListing
                    select (addLeadSuccessful, activeListing)
                )
                .Map(context =>
                {
                    _repository.Update(context.activeListing);
                    _repository.Save();

                    return unit;
                });
    }
}
