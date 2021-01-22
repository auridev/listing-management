using Common.Helpers;
using Core.Application.Profiles.Commands;
using Core.Application.Profiles.Commands.UpdateProfile;
using Core.Domain.Profiles;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Profiles.Commands.UpdateProfile
{
    public class UpdateProfileCommand_should
    {
        private UpdateProfileCommand _sut;
        private ActiveProfile _profile = DummyData.UK_Profile;
        private UpdateProfileModel _model;
        private Guid _profileId = Guid.Parse("5e495dd3-1ec6-48e6-896f-8729291b21dc");

        private AutoMocker _mocker;
        private Either<Error, Unit> _executionResult;

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
            _executionResult = _sut.Execute(_profileId, _model);
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

        private void ExecuteWith_InvalidPofileId()
        {
            _executionResult = _sut.Execute(default, _model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_profileId_is_invalid()
        {
            // act
            ExecuteWith_InvalidPofileId();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("invalid guid");
                });
        }

        [Fact]
        public void not_persist_changes_when_profileId_is_invalid()
        {
            // act
            ExecuteWith_InvalidPofileId();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_NullModel()
        {
            _executionResult = _sut.Execute(_profileId, null);
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

        private void ExecuteWith_FailedContacDetails()
        {
            _model.FirstName = string.Empty;
            _executionResult = _sut.Execute(_profileId, _model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_contact_details_failed()
        {
            // act
            ExecuteWith_FailedContacDetails();

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
        public void not_persist_changes_when_contact_details_failed()
        {
            // act
            ExecuteWith_FailedContacDetails();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_FailedLocationDetails()
        {
            _model.CountryCode = "12345";
            _executionResult = _sut.Execute(_profileId, _model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_location_details_failed()
        {
            // act
            ExecuteWith_FailedLocationDetails();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("value needs to be 2 long");
                });
        }

        [Fact]
        public void not_persist_changes_when_location_details_failed()
        {
            // act
            ExecuteWith_FailedLocationDetails();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_FailedGeographicLocation()
        {
            _model.Latitude = 1_000L;
            _executionResult = _sut.Execute(_profileId, _model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_geographic_location_failed()
        {
            // act
            ExecuteWith_FailedGeographicLocation();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("latitude out of range");
                });
        }

        [Fact]
        public void not_persist_changes_when_geographic_location_failed()
        {
            // act
            ExecuteWith_FailedGeographicLocation();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_FailedUserPreferences()
        {
            _model.DistanceUnit = "adsasdasdasd";
            _executionResult = _sut.Execute(_profileId, _model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_user_preferences_failed()
        {
            // act
            ExecuteWith_FailedUserPreferences();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("unknown distance measurement unit");
                });
        }

        [Fact]
        public void not_persist_changes_when_user_preferences_failed()
        {
            // act
            ExecuteWith_FailedUserPreferences();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_ActiveProfileNotFound()
        {
            _mocker
                .GetMock<IProfileRepository>()
                .Setup(repo => repo.Find(It.IsAny<Guid>()))
                .Returns(Option<ActiveProfile>.None);

            _executionResult = _sut.Execute(_profileId, _model);
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

        private void ExecuteWith_DefaultUserId()
        {
            _executionResult = _sut.Execute(default, _model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_user_id_is_default()
        {
            // act
            ExecuteWith_DefaultUserId();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("invalid guid");
                });
        }

        [Fact]
        public void not_persist_changes_when_user_id_is_default()
        {
            // act
            ExecuteWith_DefaultUserId();

            // assert
            VerifyChangesNotPersisted();
        }
    }
}
