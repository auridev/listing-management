using Common.Dates;
using Common.Helpers;
using Core.Application.Listings.Commands;
using Core.Application.Listings.Commands.CreateNewListing;
using Core.Application.Listings.Commands.CreateNewListing.Factory;
using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;
using static Common.Helpers.Result;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.CreateNewListing
{
    public class CreateNewListingCommand_should
    {
        private CreateNewListingCommand _sut;
        private CreateNewListingModel _model;
        private NewListing _listing;
        private Guid _userId = Guid.NewGuid();

        private AutoMocker _mocker;
        private Either<Error, Unit> _executionResult;

        public CreateNewListingCommand_should()
        {
            _mocker = new AutoMocker();

            _model = new CreateNewListingModel()
            {
                Title = "title",
                MaterialTypeId = 10,
                Weight = 2.3F,
                MassUnit = "kg",
                Description = "description",
                FirstName = "firstname",
                LastName = "lasname",
                Company = "cccc",
                Phone = "+333 111 22222",
                CountryCode = "dd",
                State = "45",
                City = "obeliai",
                PostCode = "12",
                Address = "asd",
                Latitude = 1.1D,
                Longitude = 2.2D,
                Images = new NewImageModel[]
                {
                    new NewImageModel()
                    {
                        Name = "photo1.bmp",
                        Content = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50 }
                    },
                    new NewImageModel()
                    {
                        Name = "photo2.jpg",
                        Content = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50 }
                    }
                }
            };

            var listingId = Guid.NewGuid();
            _listing = DummyData.NewListing_1;

            _mocker
                .GetMock<INewListingFactory>()
                .Setup(factory => factory.Create(
                    It.IsAny<Owner>(),
                    It.IsAny<ListingDetails>(),
                    It.IsAny<ContactDetails>(),
                    It.IsAny<LocationDetails>(),
                    It.IsAny<GeographicLocation>(),
                    It.IsAny<DateTimeOffset>()))
                .Returns(_listing);

            _mocker
                .GetMock<IDateTimeService>()
                .Setup(service => service.GetCurrentUtcDateTime())
                .Returns(DateTimeOffset.UtcNow);

            _sut = _mocker.CreateInstance<CreateNewListingCommand>();
        }

        private void ExecuteWith_Success()
        {
            _executionResult = _sut.Execute(_userId, _model);
        }

        private void VerifyChangesNotPersisted()
        {
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.IsAny<NewListing>(), It.IsAny<List<ImageReference>>()), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Never);

            _mocker
                .GetMock<IImagePersistenceService>()
                .Verify(r => r.AddAndSave(It.IsAny<Guid>(), It.IsAny<List<ImageContent>>(), It.IsAny<DateTag>()), Times.Never);
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
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.IsAny<NewListing>(), It.IsAny<List<ImageReference>>()), Times.Once);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Once);

            _mocker
                .GetMock<IImagePersistenceService>()
                .Verify(r => r.AddAndSave(It.IsAny<Guid>(), It.IsAny<List<ImageContent>>(), It.IsAny<DateTag>()), Times.Once);
        }

        private void ExecuteWith_FailedListingCreation()
        {
            _mocker
                .GetMock<INewListingFactory>()
                .Setup(factory => factory.Create(
                    It.IsAny<Owner>(),
                    It.IsAny<ListingDetails>(),
                    It.IsAny<ContactDetails>(),
                    It.IsAny<LocationDetails>(),
                    It.IsAny<GeographicLocation>(),
                    It.IsAny<DateTimeOffset>()))
                .Returns(Invalid<NewListing>("something is invalid"));

            _executionResult = _sut.Execute(_userId, _model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_listing_creation_failed()
        {
            // act
            ExecuteWith_FailedListingCreation();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("something is invalid");
                });
        }

        [Fact]
        public void not_persist_changes_when_listing_creation_failed()
        {
            // act
            ExecuteWith_FailedListingCreation();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_FailedOwner()
        {
            _executionResult = _sut.Execute(default, _model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_owner_creation_failed()
        {
            // act
            ExecuteWith_FailedOwner();

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
        public void not_persist_changes_when_owner_creation_failed()
        {
            // act
            ExecuteWith_FailedOwner();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_FailedDateTag()
        {
            _mocker
                .GetMock<IDateTimeService>()
                .Setup(service => service.GetCurrentUtcDateTime())
                .Returns((DateTimeOffset) default);

            _executionResult = _sut.Execute(_userId, _model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_date_tag_creation_failed()
        {
            // act
            ExecuteWith_FailedDateTag();

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
        public void not_persist_changes_when_date_tag_creation_failed()
        {
            // act
            ExecuteWith_FailedDateTag();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_FailedListingDetails()
        {
            _model.Title = string.Empty;

            _executionResult = _sut.Execute(_userId, _model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_listing_details_creation_failed()
        {
            // act
            ExecuteWith_FailedListingDetails();

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
        public void not_persist_changes_when_listing_details_creation_failed()
        {
            // act
            ExecuteWith_FailedListingDetails();

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
            _model.Latitude = 100D;

            _executionResult = _sut.Execute(_userId, _model);
        }


        [Fact]
        public void return_EitherLeft_with_proper_error_when_geographic_location_reation_failed()
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
