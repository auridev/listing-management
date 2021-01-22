using Common.Dates;
using Common.Helpers;
using Core.Application.Profiles.Commands;
using Core.Application.Profiles.Commands.DeactivateProfile;
using Core.Domain.Profiles;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Profiles.Commands.DeactivateProfile
{
    public class DeactivateProfileCommand_should
    {
        private DeactivateProfileCommand _sut;
        private ActiveProfile _profile = DummyData.UK_Profile;
        private DeactivateProfileModel _model;

        private AutoMocker _mocker;
        private Either<Error, Unit> _executionResult;

        public DeactivateProfileCommand_should()
        {
            _mocker = new AutoMocker();
            _model = new DeactivateProfileModel()
            {
                ActiveProfileId = new Guid("3b9a8b14-f0de-4740-a6b4-3cd6e52bd715"),
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

        private void VerifyChangesNotPersisted()
        {
            _mocker
                .GetMock<IProfileRepository>()
                .Verify(r => r.Add(It.IsAny<PassiveProfile>()), Times.Never);

            _mocker
                .GetMock<IProfileRepository>()
                .Verify(r => r.Delete(It.IsAny<ActiveProfile>()), Times.Never);

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
                .Verify(r => r.Add(It.IsAny<PassiveProfile>()), Times.Once);

            _mocker
                .GetMock<IProfileRepository>()
                .Verify(r => r.Delete(It.IsAny<ActiveProfile>()), Times.Once);

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

        private void ExecuteWith_FailedReason()
        {
            _executionResult = _sut.Execute(new DeactivateProfileModel()
            {
                ActiveProfileId = Guid.NewGuid(),
                Reason = string.Empty
            });
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_reason_creation_failed()
        {
            // act
            ExecuteWith_FailedReason();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("value cannot be empty");
                });
        }

        [Fact]
        public void not_persist_changes_when_reason_creation_failed()
        {
            // act
            ExecuteWith_FailedReason();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_ProfileNotFound()
        {
            _mocker
                .GetMock<IProfileRepository>()
                .Setup(repo => repo.Find(It.IsAny<Guid>()))
                .Returns(Option<ActiveProfile>.None);

            _executionResult = _sut.Execute(_model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_profile_was_not_found()
        {
            // act
            ExecuteWith_ProfileNotFound();

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
            ExecuteWith_ProfileNotFound();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_DeactivateFailed()
        {
            _mocker
              .GetMock<IDateTimeService>()
              .Setup(service => service.GetCurrentUtcDateTime())
              .Returns((DateTimeOffset)default);

            _executionResult = _sut.Execute(_model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_deactivate_failed()
        {
            // act
            ExecuteWith_DeactivateFailed();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("deactivationDate");
                });
        }

        [Fact]
        public void not_persist_changes_when_deactivate_failed()
        {
            // act
            ExecuteWith_DeactivateFailed();

            // assert
            VerifyChangesNotPersisted();
        }
    }
}
