using BusinessLine.Core.Application.Profiles.Commands;
using BusinessLine.Core.Application.Profiles.Commands.CreateProfile;
using BusinessLine.Core.Application.Profiles.Commands.CreateProfile.Factory;
using BusinessLine.Core.Application.UnitTests.TestMocks;
using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Profiles;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Profiles.Commands.CreateProfile
{
    public class CreateProfileCommand_should
    {
        private readonly CreateProfileCommand _sut;
        private readonly CreateProfileModel _model;
        private readonly ActiveProfile _profile;
        private readonly AutoMocker _mocker;

        public CreateProfileCommand_should()
        {
            _mocker = new AutoMocker();
            _model = new CreateProfileModel()
            {
                Email = "one@two.com",
                FirstName = "rose",
                LastName = "mary",
                Company = "facebook",
                Phone = "+333 111 22222",
                CountryCode = "dd",
                State = "sss",
                City = "utena",
                PostCode = "pcode",
                Address = "my place 1",
                Latitude = 23D,
                Longitude = 100D,
                DistanceUnit = "km",
                MassUnit = "lb",
                CurrencyCode = "eur"
            };
            _profile = ProfileMocks.UK_Profile;
            _mocker
                .GetMock<IProfileFactory>()
                .Setup(factory => factory.CreateActive(
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>(),
                    It.IsAny<Email>(),
                    It.IsAny<ContactDetails>(),
                    It.IsAny<LocationDetails>(),
                    It.IsAny<GeographicLocation>(),
                    It.IsAny<UserPreferences>()))
                .Returns(_profile);

            _sut = _mocker.CreateInstance<CreateProfileCommand>();
        }

        [Fact]
        public void add_an_active_profile_to_the_repository()
        {
            _sut.Execute(Guid.NewGuid(), _model);

            _mocker
                .GetMock<IProfileRepository>()
                .Verify(r => r.Add(_profile), Times.Once); // Add entity returned by the factory
        }

        [Fact]
        public void save_repository_changes()
        {
            _sut.Execute(Guid.NewGuid(), _model);

            _mocker
                .GetMock<IProfileRepository>()
                .Verify(r => r.Save(), Times.Once);
        }
    }
}
