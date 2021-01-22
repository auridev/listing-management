using Common.Dates;
using Common.Helpers;
using Core.Application.Profiles.Commands;
using Core.Application.Profiles.Commands.MarkProfileAsIntroduced;
using Core.Domain.Profiles;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Profiles.Commands.MarkProfileAsIntroduced
{
    public class MarkProfileAsIntroducedCommand_should
    {
        private MarkProfileAsIntroducedCommand _sut;
        private MarkProfileAsIntroducedModel _model;
        private ActiveProfile _activeProfile;
        private Guid _profileId = Guid.NewGuid();
        private DateTimeOffset _seenDate = DateTimeOffset.UtcNow;

        private AutoMocker _mocker;
        private Either<Error, Unit> _executionResult;

        public MarkProfileAsIntroducedCommand_should()
        {
            _mocker = new AutoMocker();
            _activeProfile = DummyData.UK_Profile;
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
                .Setup(r => r.Find(It.IsAny<Guid>()))
                .Returns(_activeProfile);

            _sut = _mocker.CreateInstance<MarkProfileAsIntroducedCommand>();
        }

        private void VerifyChangesNotPersisted()
        {
            _mocker
                .GetMock<IProfileRepository>()
                .Verify(r => r.Update(It.IsAny<ActiveProfile>()), Times.Never);

            _mocker
                .GetMock<IProfileRepository>()
                .Verify(r => r.Save(), Times.Never);
        }

        private void ExecuteWith_Success()
        {
            _executionResult = _sut.Execute(_model);
        }

        [Fact]
        public void return_EitherRight_on_success()
        {
            // act
            ExecuteWith_Success();

            // assert
            _executionResult
                .Right(u => u.Should().NotBeNull())
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void persist_changes_on_success()
        {
            // act
            ExecuteWith_Success();

            // assert
            _mocker
                .GetMock<IProfileRepository>()
                .Verify(r => r.Update(It.IsAny<ActiveProfile>()), Times.Once);

            _mocker
                .GetMock<IProfileRepository>()
                .Verify(r => r.Save(), Times.Once);
        }

        private void ExecuteWith_NullModel()
        {
            _executionResult = _sut.Execute(null);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_model_is_null()
        {
            // act
            ExecuteWith_NullModel();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("cannot be null");
                });
        }

        [Fact]
        public void not_persist_changes_when_model_is_null()
        {
            // act
            ExecuteWith_NullModel();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_FailedSeenDate()
        {
            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetCurrentUtcDateTime())
                .Returns((DateTimeOffset)default);

            _executionResult = _sut.Execute(_model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_seen_date_creation_failed()
        {
            // act
            ExecuteWith_FailedSeenDate();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("invalid dateTimeOffset");
                });
        }

        [Fact]
        public void not_persist_changes_when_seen_date_creation_failed()
        {
            // act
            ExecuteWith_FailedSeenDate();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_ActiveProfileNotFound()
        {
            _mocker
                .GetMock<IProfileRepository>()
                .Setup(r => r.Find(It.IsAny<Guid>()))
                .Returns(Option<ActiveProfile>.None);

            _executionResult = _sut.Execute(_model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_profile_was_not_found()
        {
            // act
            ExecuteWith_ActiveProfileNotFound();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.NotFound>();
                    error.Message.Should().Be("active profile not found");
                });
        }

        [Fact]
        public void not_persist_changes_when_profile_was_not_found()
        {
            // act
            ExecuteWith_ActiveProfileNotFound();

            // assert
            VerifyChangesNotPersisted();
        }
    }
}
