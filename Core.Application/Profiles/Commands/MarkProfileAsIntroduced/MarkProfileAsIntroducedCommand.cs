using Common.Dates;
using Common.Helpers;
using Core.Domain.Profiles;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using static Common.Helpers.Functions;
using static LanguageExt.Prelude;

namespace Core.Application.Profiles.Commands.MarkProfileAsIntroduced
{
    public sealed class MarkProfileAsIntroducedCommand : IMarkProfileAsIntroducedCommand
    {
        private readonly IProfileRepository _repository;
        private readonly IDateTimeService _dateTimeService;

        public MarkProfileAsIntroducedCommand(IProfileRepository repository, IDateTimeService dateTimeService)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public Either<Error, Unit> Execute(MarkProfileAsIntroducedModel model)
        {
            Either<Error, MarkProfileAsIntroducedModel> eitherModel = EnsureNotNull(model);
            Either<Error, ActiveProfile> eitherProfile = FindProfile(eitherModel);
            Either<Error, SeenDate> eitherSeenDate =
                CreateSeenDate(
                    eitherProfile,
                    _dateTimeService.GetCurrentUtcDateTime());
            Either<Error, Unit> eitherHasSeenIntroduction =
                HasSeenIntroduction(
                    eitherProfile,
                    eitherSeenDate);
            Either<Error, Unit> persistChangesResult =
                PersistChanges(
                    eitherHasSeenIntroduction,
                    eitherProfile);

            return persistChangesResult;
        }

        private Either<Error, ActiveProfile> FindProfile(Either<Error, MarkProfileAsIntroducedModel> eitherModel)
            =>
                eitherModel
                    .Map(model => _repository.Find(model.ProfileId))
                    .Bind(option => option.ToEither<Error>(new Error.NotFound("active profile not found")));

        private Either<Error, SeenDate> CreateSeenDate(Either<Error, ActiveProfile> eitherActiveProfile, DateTimeOffset seenDate)
            =>
                eitherActiveProfile
                    .Bind(model => SeenDate.Create(seenDate));

        private Either<Error, Unit> HasSeenIntroduction(Either<Error, ActiveProfile> eitherActiveProfile, Either<Error, SeenDate> eitherSeenDate)
            =>
                (
                    from activeProfile in eitherActiveProfile
                    from seenDate in eitherSeenDate
                    select
                        (activeProfile, seenDate)
                )
                .Bind(context =>
                        context.activeProfile.HasSeenIntroduction(context.seenDate));

        private Either<Error, Unit> PersistChanges(Either<Error, Unit> eitherHasSeenIndtrocution, Either<Error, ActiveProfile> eitherActiveProfile)
            =>
                (
                    from hasSeenIndtrocution in eitherHasSeenIndtrocution
                    from activeProfile in eitherActiveProfile
                    select
                        (hasSeenIndtrocution, activeProfile)
                )
                .Map(context =>
                {
                    _repository.Update(context.activeProfile);
                    _repository.Save();

                    return unit;
                });
    }
}
