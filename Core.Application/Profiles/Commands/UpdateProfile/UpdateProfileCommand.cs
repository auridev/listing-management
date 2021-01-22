using Common.Helpers;
using Core.Domain.Profiles;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using static Common.Helpers.Functions;
using static LanguageExt.Prelude;

namespace Core.Application.Profiles.Commands.UpdateProfile
{
    public sealed class UpdateProfileCommand : IUpdateProfileCommand
    {
        private readonly IProfileRepository _repository;
        public UpdateProfileCommand(IProfileRepository repository)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        public Either<Error, Unit> Execute(Guid profileId, UpdateProfileModel model)
        {
            Either<Error, Guid> eitherProfileId = EnsureNonDefault(profileId);
            Either<Error, UpdateProfileModel> eitherModel = EnsureNotNull(model);
            Either<Error, ActiveProfile> eitherActiveProfile = FindProfile(eitherProfileId);
            Either<Error, ContactDetails> eitherContactDetails = CreateContactDetails(eitherModel);
            Either<Error, LocationDetails> eitherLocationDetails = CreateLocationDetails(eitherModel);
            Either<Error, GeographicLocation> eitherGeographicLocation = CreateGeographicLocation(eitherModel);
            Either<Error, UserPreferences> eitherUserPreferences = CreateUserPreferences(eitherModel);

            Either<Error, Unit> eitherUpdateDetails =
                UpdateDetails(
                    eitherActiveProfile,
                    eitherContactDetails,
                    eitherLocationDetails,
                    eitherGeographicLocation,
                    eitherUserPreferences);
            Either<Error, Unit> persistChangesResult =
                PersistChanges(
                    eitherUpdateDetails,
                    eitherActiveProfile);

            return persistChangesResult;
        }

        private Either<Error, ActiveProfile> FindProfile(Either<Error, Guid> eitherProfileId)
            =>
                eitherProfileId
                    .Map(profileId => _repository.Find(profileId))
                    .Bind(option => option.ToEither<Error>(new Error.NotFound("active profile not found")));

        private Either<Error, ContactDetails> CreateContactDetails(Either<Error, UpdateProfileModel> eitherModel)
            =>
                eitherModel
                    .Bind(model =>
                        ContactDetails.Create(
                            model.FirstName,
                            model.LastName,
                            model.Company,
                            model.Phone));

        private Either<Error, LocationDetails> CreateLocationDetails(Either<Error, UpdateProfileModel> eitherModel)
            =>
                eitherModel
                    .Bind(model =>
                        LocationDetails.Create(
                            model.CountryCode,
                            model.State,
                            model.City,
                            model.PostCode,
                            model.Address));

        private Either<Error, GeographicLocation> CreateGeographicLocation(Either<Error, UpdateProfileModel> eitherModel)
            =>
                eitherModel
                    .Bind(model =>
                        GeographicLocation.Create(
                           model.Latitude,
                            model.Longitude));

        private Either<Error, UserPreferences> CreateUserPreferences(Either<Error, UpdateProfileModel> eitherModel)
            =>
                eitherModel
                    .Bind(model =>
                        UserPreferences.Create(
                            model.DistanceUnit,
                            model.MassUnit,
                            model.CurrencyCode));

        private Either<Error, Unit> UpdateDetails(
            Either<Error, ActiveProfile> eitherActiveProfile,
            Either<Error, ContactDetails> eitherContactDetails,
            Either<Error, LocationDetails> eitherLocationDetails,
            Either<Error, GeographicLocation> eitherGeographicLocation,
            Either<Error, UserPreferences> eitherUserPreferences)
            =>
                (
                    from activeProfile in eitherActiveProfile
                    from contactDetails in eitherContactDetails
                    from locationDetails in eitherLocationDetails
                    from geographicLocation in eitherGeographicLocation
                    from userPreferences in eitherUserPreferences
                    select
                        (activeProfile, contactDetails, locationDetails, geographicLocation, userPreferences)
                )
                .Bind(context =>
                        context.activeProfile.UpdateDetails(
                            context.contactDetails,
                            context.locationDetails,
                            context.geographicLocation,
                            context.userPreferences));

        private Either<Error, Unit> PersistChanges(Either<Error, Unit> eitherUpdateDetails, Either<Error, ActiveProfile> eitherActiveProfile)
            =>
                (
                    from updateDetails in eitherUpdateDetails
                    from activeProfile in eitherActiveProfile
                    select
                        (updateDetails, activeProfile)
                )
                .Map(context =>
                {
                    _repository.Update(context.activeProfile);
                    _repository.Save();

                    return unit;
                });
    }
}
