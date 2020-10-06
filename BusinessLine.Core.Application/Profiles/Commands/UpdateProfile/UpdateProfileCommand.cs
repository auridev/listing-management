using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Profiles;
using LanguageExt;
using System;

namespace BusinessLine.Core.Application.Profiles.Commands.UpdateProfile
{
    public sealed class UpdateProfileCommand : IUpdateProfileCommand
    {
        private readonly IProfileRepository _repository;
        public UpdateProfileCommand(IProfileRepository repository)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        public void Execute(Guid profileId, UpdateProfileModel model)
        {
            // Prerequisites
            var newContactDetails = ContactDetails.Create(
                PersonName.Create(model.FirstName, model.LastName),
                Company.Create(model.Company),
                Phone.Create(model.Phone));

            var newLocationDetails = LocationDetails.Create(
                Alpha2Code.Create(model.CountryCode),
                State.Create(model.State),
                City.Create(model.City),
                PostCode.Create(model.PostCode),
                Address.Create(model.Address));

            var newGeographicLocation = GeographicLocation.Create(
                model.Latitude,
                model.Longitude);

            var newUserPreferences = UserPreferences.Create(
                DistanceMeasurementUnit.BySymbol(model.DistanceUnit),
                MassMeasurementUnit.BySymbol(model.MassUnit),
                CurrencyCode.Create(model.CurrencyCode));

            // Command
            Option<ActiveProfile> optionalProfile = _repository.Find(profileId);


            optionalProfile
                .Some(profile =>
                {
                    profile.UpdateDetails(newContactDetails,
                        newLocationDetails,
                        newGeographicLocation,
                        newUserPreferences);

                    _repository.Update(profile); // not strictly required but improves readability

                    _repository.Save();
                })
                .None(() => { });
        }
    }
}
