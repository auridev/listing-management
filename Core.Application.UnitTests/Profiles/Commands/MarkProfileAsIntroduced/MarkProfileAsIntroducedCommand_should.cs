using Common.Dates;
using Core.Application.Profiles.Commands;
using Core.Application.Profiles.Commands.MarkProfileAsIntroduced;
using Core.Domain.Profiles;
using Core.UnitTests.Mocks;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Profiles.Commands.MarkProfileAsIntroduced
{
    public class MarkProfileAsIntroducedCommand_should
    {
        private readonly MarkProfileAsIntroducedCommand _sut;
        private readonly MarkProfileAsIntroducedModel _model;
        private readonly ActiveProfile _activeProfile;
        private readonly Guid _profileId = Guid.NewGuid();
        private readonly DateTimeOffset _seenDate = DateTimeOffset.UtcNow;
        private readonly AutoMocker _mocker;

        public MarkProfileAsIntroducedCommand_should()
        {
            _mocker = new AutoMocker();
            _activeProfile = FakesCollection.UK_Profile;
            _model = new MarkProfileAsIntroducedModel()
            {
                ProfileId = _profileId
            };

            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetCurrentUtcDateTime())
                .Returns(DateTimeOffset.UtcNow);

            _mocker
                .GetMock<IProfileRepository>()
                .Setup(r => r.Find(_profileId))
                .Returns(_activeProfile);

            _sut = _mocker.CreateInstance<MarkProfileAsIntroducedCommand>();
        }

        [Fact]
        public void retrieve_active_profile_from_repo()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IProfileRepository>()
                .Verify(r => r.Find(_profileId), Times.Once);
        }

        [Fact]
        public void save_chages_to_repo()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IProfileRepository>()
                .Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void do_nothing_if_profile_doesnt_exist()
        {
            _mocker
                .GetMock<IProfileRepository>()
                .Setup(r => r.Find(_profileId))
                .Returns(Option<ActiveProfile>.None);

            _sut.Execute(_model);

            _mocker
                .GetMock<IProfileRepository>()
                .Verify(r => r.Save(), Times.Never);
        }

    }
}
