using Common.Dates;
using Common.Helpers;
using Core.Application.Profiles.Commands.CreateProfile.Factory;
using Core.Domain.ValueObjects;
using System;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Core.Application.Profiles.Commands.CreateProfile
{
    public sealed class CreateProfileCommand : ICreateProfileCommand
    {
        private readonly IProfileRepository _repository;
        private readonly IProfileFactory _factory;
        private readonly IDateTimeService _dateTimeService;

        public CreateProfileCommand(IProfileRepository repository, IProfileFactory factory, IDateTimeService dateTimeService)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            _factory = factory ??
                throw new ArgumentNullException(nameof(factory));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public void Execute(Guid userid, CreateProfileModel model)
        {
            // Pre-requisites
            var email = Email.Create(model.Email);

            // cia neteisingai
            var optionalCompany = Company.Create(model.Company)
                .Match(
                    rightValue => Right(Option<Company>.Some(rightValue)),
                    leftValue => Right(Option<Company>.None));

            var contactDetails = ContactDetails.Create(
                PersonName.Create(model.FirstName, model.LastName),
                optionalCompany,
                Phone.Create(model.Phone));

            var locationDetails = LocationDetails.Create(
                Alpha2Code.Create(model.CountryCode).ToUnsafeRight(),
                Domain.ValueObjects.State.Create(model.State),
                City.Create(model.City).ToUnsafeRight(),
                PostCode.Create(model.PostCode),
                Address.Create(model.Address).ToUnsafeRight());

            var geographicLocation = GeographicLocation.Create(
                model.Latitude,
                model.Longitude);

            var userPreferences = UserPreferences.Create(
                DistanceMeasurementUnit.BySymbol(model.DistanceUnit),
                MassMeasurementUnit.BySymbol(model.MassUnit),
                CurrencyCode.Create(model.CurrencyCode));

            var createdDate = _dateTimeService.GetCurrentUtcDateTime();

            // Command
            var activeProfile = _factory.CreateActive(
                Guid.NewGuid(),
                userid,
                email,
                contactDetails.ToUnsafeRight(),
                locationDetails,
                geographicLocation,
                userPreferences,
                createdDate);

            _repository.Add(activeProfile);

            _repository.Save();
        }
    }
}
