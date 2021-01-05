using Core.Domain.ValueObjects;
using Core.Domain.Profiles;
using LanguageExt;
using System;
using Common.Helpers;
using LanguageExt;
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

        public void Execute(Guid profileId, UpdateProfileModel model)
        {
            // Prerequisites
            var optionalCompany = Company.Create(model.Company)
                    .Match(
                        rightValue => Right(Option<Company>.Some(rightValue)),
                        leftValue => Right(Option<Company>.None));

            var newContactDetails = ContactDetails.Create(
                PersonName.Create(model.FirstName, model.LastName),
                optionalCompany,
                Phone.Create(model.Phone));

            var newLocationDetails = LocationDetails.Create(
                Alpha2Code.Create(model.CountryCode).ToUnsafeRight(),
                Domain.ValueObjects.State.Create(model.State),
                City.Create(model.City).ToUnsafeRight(),
                PostCode.Create(model.PostCode),
                Address.Create(model.Address).ToUnsafeRight());

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
                    profile.UpdateDetails(newContactDetails.ToUnsafeRight(),
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
