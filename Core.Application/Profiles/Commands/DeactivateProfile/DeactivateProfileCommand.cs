using Common.Dates;
using Common.Helpers;
using Core.Domain.Profiles;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using static Common.Helpers.Functions;
using static LanguageExt.Prelude;

namespace Core.Application.Profiles.Commands.DeactivateProfile
{
    public sealed class DeactivateProfileCommand : IDeactivateProfileCommand
    {
        private readonly IProfileRepository _repository;
        private readonly IDateTimeService _dateTimeService;
        public DeactivateProfileCommand(IProfileRepository repository, IDateTimeService dateTimeService)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public Either<Error, Unit> Execute(DeactivateProfileModel model)
        {
            Either<Error, DeactivateProfileModel> eitherModel = EnsureNotNull(model);
            Either<Error, ActiveProfile> eitherProfile = FindProfile(eitherModel);
            Either<Error, TrimmedString> eitherReason = CreateReason(eitherModel);

            Either<Error, PassiveProfile> eitherPassiveProfile =
                Deactivate(
                    eitherProfile,
                    eitherReason,
                    _dateTimeService.GetCurrentUtcDateTime());
            Either<Error, Unit> persistChangesResult =
                PersistChanges(
                    eitherProfile,
                    eitherPassiveProfile);

            return persistChangesResult;
        }

        private Either<Error, ActiveProfile> FindProfile(Either<Error, DeactivateProfileModel> eitherModel)
            =>
                eitherModel
                    .Map(model => _repository.Find(model.ActiveProfileId))
                    .Bind(option => option.ToEither<Error>(new Error.NotFound("active profile not found")));

        private Either<Error, TrimmedString> CreateReason(Either<Error, DeactivateProfileModel> eitherModel)
            =>
                eitherModel
                    .Bind(model => TrimmedString.Create(model.Reason));


        private Either<Error, PassiveProfile> Deactivate(Either<Error, ActiveProfile> eitherActiveProfile, Either<Error, TrimmedString> eitherReason, DateTimeOffset deactivationDate)
            =>
                (
                    from activeProfile in eitherActiveProfile
                    from reason in eitherReason
                    select
                        (activeProfile, reason)
                )
                .Bind(context =>
                        context.activeProfile.Deactivate(deactivationDate, context.reason));

        private Either<Error, Unit> PersistChanges(Either<Error, ActiveProfile> eitherActiveProfile, Either<Error, PassiveProfile> eitherPassiveProfile)
            =>
                (
                    from activeProfile in eitherActiveProfile
                    from passiveProfile in eitherPassiveProfile
                    select
                        (activeProfile, passiveProfile)
                )
                .Map(context =>
                {
                    _repository.Add(context.passiveProfile);
                    _repository.Delete(context.activeProfile);
                    _repository.Save();

                    return unit;
                });
    }
}
