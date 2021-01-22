using Common.Helpers;
using Core.Application.Profiles.Commands;
using Core.Application.Profiles.Commands.CreateProfile;
using Core.Application.Profiles.Commands.CreateProfile.Factory;
using Core.Domain.Profiles;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Test.Helpers;
using Xunit;
using static LanguageExt.Prelude;

namespace BusinessLine.Core.Application.UnitTests.Profiles.Commands.CreateProfile
{
    public class CreateProfileCommand_should
    {
        private CreateProfileCommand _sut;
        private CreateProfileModel _model;
        private ActiveProfile _profile;
        private Guid _userId = Guid.NewGuid();

        private AutoMocker _mocker;
        private Either<Error, Unit> _executionResult;

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
            _profile = DummyData.UK_Profile;
            _mocker
                .GetMock<IProfileFactory>()
                .Setup(factory => factory.CreateActive(
                    It.IsAny<Guid>(),
                    It.IsAny<Email>(),
                    It.IsAny<ContactDetails>(),
                    It.IsAny<LocationDetails>(),
                    It.IsAny<GeographicLocation>(),
                    It.IsAny<UserPreferences>()))
                .Returns(_profile);

            _sut = _mocker.CreateInstance<CreateProfileCommand>();
        }

        private void VerifyChangesNotPersisted()
        {
            _mocker
                .GetMock<IProfileRepository>()
                .Verify(r => r.Add(It.IsAny<ActiveProfile>()), Times.Never);

            _mocker
                .GetMock<IProfileRepository>()
                .Verify(r => r.Save(), Times.Never);
        }

        private void ExecuteWith_Success()
        {
            _executionResult = _sut.Execute(_userId, _model);
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
                .Verify(r => r.Add(It.IsAny<ActiveProfile>()), Times.Once);

            _mocker
                .GetMock<IProfileRepository>()
                .Verify(r => r.Save(), Times.Once);
        }

        private void ExecuteWith_FailedEmail()
        {
            _model.Email = string.Empty;
            _executionResult = _sut.Execute(_userId, _model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_email_creation_failed()
        {
            // act
            ExecuteWith_FailedEmail();

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
        public void not_persist_changes_when_email_creation_failed()
        {
            // act
            ExecuteWith_FailedEmail();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_FailedContactDetails()
        {
            _model.FirstName = string.Empty;
            _executionResult = _sut.Execute(_userId, _model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_contact_details_creation_failed()
        {
            // act
            ExecuteWith_FailedContactDetails();

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
        public void not_persist_changes_when_contact_details_creation_failed()
        {
            // act
            ExecuteWith_FailedContactDetails();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_FailedLocationDetails()
        {
            _model.CountryCode = string.Empty;
            _executionResult = _sut.Execute(_userId, _model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_location_details_creation_failed()
        {
            // act
            ExecuteWith_FailedLocationDetails();

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
        public void not_persist_changes_when_location_details_creation_failed()
        {
            // act
            ExecuteWith_FailedLocationDetails();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_FailedGeographicLocation()
        {
            _model.Latitude = 100L;
            _executionResult = _sut.Execute(_userId, _model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_geographic_location_creation_failed()
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
        public void not_persist_changes_when_geographic_location_creation_failed()
        {
            // act
            ExecuteWith_FailedGeographicLocation();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_FailedUserPreferences()
        {
            _model.DistanceUnit = "asd";
            _executionResult = _sut.Execute(_userId, _model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_user_preferences_creation_failed()
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
        public void not_persist_changes_when_user_preferences_creation_failed()
        {
            // act
            ExecuteWith_FailedUserPreferences();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_FailedProfile()
        {
            _mocker
               .GetMock<IProfileFactory>()
               .Setup(factory => factory.CreateActive(
                   It.IsAny<Guid>(),
                   It.IsAny<Email>(),
                   It.IsAny<ContactDetails>(),
                   It.IsAny<LocationDetails>(),
                   It.IsAny<GeographicLocation>(),
                   It.IsAny<UserPreferences>()))
               .Returns(Left<Error, ActiveProfile>(new Error.Invalid("some invalid profile")));

            _executionResult = _sut.Execute(_userId, _model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_profile_creation_failed()
        {
            // act
            ExecuteWith_FailedProfile();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("some invalid profile");
                });
        }

        [Fact]
        public void not_persist_changes_when_profile_creation_failed()
        {
            // act
            ExecuteWith_FailedProfile();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_InvalidUserId()
        {
            _executionResult = _sut.Execute(default, _model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_user_id_is_invalid()
        {
            // act
            ExecuteWith_InvalidUserId();

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
        public void not_persist_changes_when_user_id_is_invalid()
        {
            // act
            ExecuteWith_InvalidUserId();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_NullModel()
        {
            _executionResult = _sut.Execute(_userId, null);
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
    }
}
