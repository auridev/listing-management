using BusinessLine.Common.Dates;
using BusinessLine.Core.Application.Profiles.Commands;
using BusinessLine.Core.Application.Profiles.Commands.DeactivateProfile;
using BusinessLine.Core.Application.UnitTests.TestMocks;
using BusinessLine.Core.Domain.Profiles;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Profiles.Commands.DeactivateProfile
{
    public class DeactivateProfileCommand_should
    {
        private readonly DeactivateProfileCommand _sut;
        private readonly AutoMocker _mocker;
        private readonly ActiveProfile _profile = ProfileMocks.UK_Profile;
        private readonly DeactivateProfileModel _model;

        public DeactivateProfileCommand_should()
        {
            _mocker = new AutoMocker();
            _model = new DeactivateProfileModel()
            {
                ActiveProfileId = "3b9a8b14-f0de-4740-a6b4-3cd6e52bd715",
                Reason = "because I want all"
            };

            _mocker
              .GetMock<IDateTimeService>()
              .Setup(service => service.GetCurrentUtcDateTime())
              .Returns(DateTimeOffset.UtcNow);

            _mocker
                .GetMock<IProfileRepository>()
                .Setup(repo => repo.Find(It.IsAny<Guid>()))
                .Returns(Option<ActiveProfile>.Some(_profile));

            _sut = _mocker.CreateInstance<DeactivateProfileCommand>();
        }

        [Fact]
        public void retrieve_the_active_profile_from_repository()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IProfileRepository>()
                .Verify(r => r.Find(Guid.Parse("3b9a8b14-f0de-4740-a6b4-3cd6e52bd715")), Times.Once);
        }

        [Fact]
        public void add_passive_profile_to_the_repository()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IProfileRepository>()
                .Verify(r => r.Add(It.IsNotNull<PassiveProfile>()), Times.Once);
        }

        [Fact]
        public void remove_active_profile_from_the_repository()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IProfileRepository>()
                .Verify(r => r.Delete(_profile), Times.Once);
        }

        [Fact]
        public void save_repository_changes()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IProfileRepository>()
                .Verify(r => r.Save(), Times.Once);
        }
    }
}
