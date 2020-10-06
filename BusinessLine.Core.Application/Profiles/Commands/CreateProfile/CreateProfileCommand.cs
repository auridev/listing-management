using BusinessLine.Core.Application.Profiles.Commands.CreateProfile.Factory;
using BusinessLine.Core.Domain.Common;
using System;

namespace BusinessLine.Core.Application.Profiles.Commands.CreateProfile
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

        public void Execute(Guid userid, CreateProfileModel model)
        {
            // Pre-requisites
            var email = Email.Create(model.Email);

            var contactDetails = ContactDetails.Create(
                PersonName.Create(model.FirstName, model.LastName),
                Company.Create(model.Company),
                Phone.Create(model.Phone));

            var locationDetails = LocationDetails.Create(
                Alpha2Code.Create(model.CountryCode),
                State.Create(model.State),
                City.Create(model.City),
                PostCode.Create(model.PostCode),
                Address.Create(model.Address));

            var geographicLocation = GeographicLocation.Create(
                model.Latitude,
                model.Longitude);

            var userPreferences = UserPreferences.Create(
                DistanceMeasurementUnit.BySymbol(model.DistanceUnit),
                MassMeasurementUnit.BySymbol(model.MassUnit),
                CurrencyCode.Create(model.CurrencyCode));

            // Command
            var activeProfile = _factory.CreateActive(
                Guid.NewGuid(),
                userid,
                email,
                contactDetails,
                locationDetails,
                geographicLocation,
                userPreferences);

            _repository.Add(activeProfile);

            _repository.Save();
        }
    }
}
