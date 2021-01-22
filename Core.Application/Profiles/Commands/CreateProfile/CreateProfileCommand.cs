using Common.Helpers;
using Core.Application.Profiles.Commands.CreateProfile.Factory;
using Core.Domain.Profiles;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using static Common.Helpers.Functions;
using static LanguageExt.Prelude;

namespace Core.Application.Profiles.Commands.CreateProfile
{
    public sealed class CreateProfileCommand : ICreateProfileCommand
    {
        private readonly IProfileRepository _repository;
        private readonly IProfileFactory _factory;

        public CreateProfileCommand(IProfileRepository repository, IProfileFactory factory)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            _factory = factory ??
                throw new ArgumentNullException(nameof(factory));
        }

        public Either<Error, Unit> Execute(Guid userid, CreateProfileModel model)
        {
            Either<Error, Guid> eitherUserId = EnsureNonDefault(userid);
            Either<Error, CreateProfileModel> eitherModel = EnsureNotNull(model);
            Either<Error, Email> eitherEmail = CreateEmail(eitherModel);
            Either<Error, ContactDetails> eitherContactDetails = CreateContactDetails(eitherModel);
            Either<Error, LocationDetails> eitherLocationDetails = CreateLocationDetails(eitherModel);
            Either<Error, GeographicLocation> eitherGeographicLocation = CreateGeographicLocation(eitherModel);
            Either<Error, UserPreferences> eitherUserPreferences = CreateUserPreferences(eitherModel);

            Either<Error, Unit> result =
                CreateActiveProfile(
                    eitherUserId,
                    eitherEmail,
                    eitherContactDetails,
                    eitherLocationDetails,
                    eitherGeographicLocation,
                    eitherUserPreferences)
                .Bind(
                    profile => PersistChanges(profile));

            return result;
        }

        private Either<Error, Email> CreateEmail(Either<Error, CreateProfileModel> eitherModel)
            =>
                eitherModel
                    .Bind(model =>
                        Email.Create(model.Email));

        private Either<Error, ContactDetails> CreateContactDetails(Either<Error, CreateProfileModel> eitherModel)
            =>
                eitherModel
                    .Bind(model =>
                        ContactDetails.Create(
                            model.FirstName,
                            model.LastName,
                            model.Company,
                            model.Phone));

        private Either<Error, LocationDetails> CreateLocationDetails(Either<Error, CreateProfileModel> eitherModel)
            =>
                eitherModel
                    .Bind(model =>
                        LocationDetails.Create(
                            model.CountryCode,
                            model.State,
                            model.City,
                            model.PostCode,
                            model.Address));

        private Either<Error, GeographicLocation> CreateGeographicLocation(Either<Error, CreateProfileModel> eitherModel)
            =>
                eitherModel
                    .Bind(model =>
                        GeographicLocation.Create(model.Latitude, model.Longitude));

        private Either<Error, UserPreferences> CreateUserPreferences(Either<Error, CreateProfileModel> eitherModel)
            =>
                eitherModel
                    .Bind(model =>
                        UserPreferences.Create(
                            model.DistanceUnit,
                            model.MassUnit,
                            model.CurrencyCode));

        private Either<Error, ActiveProfile> CreateActiveProfile(
            Either<Error, Guid> eitherUserId,
            Either<Error, Email> eitherEmail,
            Either<Error, ContactDetails> eitherContactDetails,
            Either<Error, LocationDetails> eitherLocationDetails,
            Either<Error, GeographicLocation> eitherGeographicLocation,
            Either<Error, UserPreferences> eitherUserPreferences)
            =>
                (
                    from userId in eitherUserId
                    from email in eitherEmail
                    from contactDetails in eitherContactDetails
                    from locationDetails in eitherLocationDetails
                    from geographicLocation in eitherGeographicLocation
                    from userPreferences in eitherUserPreferences
                    select
                        (userId, email, contactDetails, locationDetails, geographicLocation, userPreferences)
                )
                .Bind(
                    context =>
                        _factory.CreateActive(
                            context.userId,
                            context.email,
                            context.contactDetails,
                            context.locationDetails,
                            context.geographicLocation,
                            context.userPreferences));

        private Either<Error, Unit> PersistChanges(Either<Error, ActiveProfile> eitherActiveProfile)
            =>
                eitherActiveProfile
                    .Map(profile =>
                    {
                        _repository.Add(profile);
                        _repository.Save();

                        return unit;
                    });
    }
}
