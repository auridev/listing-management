using BusinessLine.Core.Application.Profiles.Commands;
using BusinessLine.Core.Application.Profiles.Commands.UpdateProfile;
using BusinessLine.Core.Application.UnitTests.TestMocks;
using BusinessLine.Core.Domain.Profiles;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Profiles.Commands.UpdateProfile
{
    public class UpdateProfileCommand_should
    {
        private readonly UpdateProfileCommand _sut;
        private readonly AutoMocker _mocker;
        private readonly ActiveProfile _profile = ProfileMocks.UK_Profile;
        private readonly UpdateProfileModel _model;
        private readonly Guid _profileId = Guid.Parse("5e495dd3-1ec6-48e6-896f-8729291b21dc");

        public UpdateProfileCommand_should()
        {
            _mocker = new AutoMocker();
            _model = new UpdateProfileModel()
            {
                FirstName = "mike",
                LastName = "pike",
                Company = null,
                Phone = "+333 111 22222",
                CountryCode = "uk",
                State = null,
                City = "looodon",
                PostCode = "g32",
                Address = "my place 1",
                Latitude = 12.5D,
                Longitude = 78.5D,
                DistanceUnit = "km",
                MassUnit = "lb",
                CurrencyCode = "gbp"
            };

            _mocker
                .GetMock<IProfileRepository>()
                .Setup(repo => repo.Find(It.IsAny<Guid>()))
                .Returns(Option<ActiveProfile>.Some(_profile));

            _sut = _mocker.CreateInstance<UpdateProfileCommand>();
        }

        [Fact]
        public void retrieve_the_active_profile_from_repository()
        {
            _sut.Execute(_profileId, _model);

            _mocker
                .GetMock<IProfileRepository>()
                .Verify(r => r.Find(_profileId), Times.Once);
        }

        [Fact]
        public void update_repository_with_the_changed_profile()
        {
            _sut.Execute(_profileId, _model);

            _mocker
                .GetMock<IProfileRepository>()
                .Verify(r => r.Update(
                    It.Is<ActiveProfile>(p => p.ContactDetails.PersonName.FirstName.Value.Equals("Mike"))),
                Times.Once);
        }

        [Fact]
        public void save_repository_changes()
        {
            _sut.Execute(_profileId, _model);

            _mocker
                .GetMock<IProfileRepository>()
                .Verify(r => r.Save(), Times.Once);
        }
    }
}
