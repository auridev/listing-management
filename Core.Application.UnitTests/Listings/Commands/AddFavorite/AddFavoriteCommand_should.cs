using Common.Dates;
using Common.Helpers;
using Core.Application.Listings.Commands;
using Core.Application.Listings.Commands.AddFavorite;
using Core.Domain.Listings;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.AddFavorite
{
    public class AddFavoriteCommand_should
    {
        private AddFavoriteCommand _sut;
        private AddFavoriteModel _model;
        private Guid _userId = Guid.NewGuid();
        private ActiveListing _activeListing;

        private AutoMocker _mocker;
        private Either<Error, Unit> _executionResult;

        public AddFavoriteCommand_should()
        {
            _mocker = new AutoMocker();
            _model = _mocker.CreateInstance<AddFavoriteModel>();
            _activeListing = DummyData.ActiveListing_1;

            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindActive(It.IsAny<Guid>()))
                .Returns(Option<ActiveListing>.Some(_activeListing));

            _mocker
                .GetMock<IDateTimeService>()
                .Setup(r => r.GetCurrentUtcDateTime())
                .Returns(DateTimeOffset.UtcNow);

            _sut = _mocker.CreateInstance<AddFavoriteCommand>();
        }

        private void Execute_Successfully()
        {
            _executionResult = _sut.Execute(_userId, _model);
        }

        private void Execute_WithNonExistingListing()
        {
            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindActive(It.IsAny<Guid>()))
                .Returns(Option<ActiveListing>.None);

            _executionResult = _sut.Execute(_userId, _model);
        }

        private void Execute_WithFailedFavoriteMarkCreation()
        {
            // this forces FavoriteMark value object creation to fail
            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetCurrentUtcDateTime())
                .Returns((DateTimeOffset)default);

            _executionResult = _sut.Execute(_userId, _model);
        }

        private void Execute_WithFailedMarkAsFavorite()
        {
            // trying to mark a listing as favorite by listing owner forces MarkAsFavorite to fail
            var listingOwnerId = _activeListing.Owner.UserId;
            _executionResult = _sut.Execute(listingOwnerId, _model);
        }

        private void VerifyChangesNotPersisted()
        {
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Update(It.IsAny<ActiveListing>()), Times.Never);
        }

        [Fact]
        public void return_EitherRight_on_success()
        {
            // act
            Execute_Successfully();

            // assert
            _executionResult
                .Right(u => u.Should().NotBeNull())
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void update_active_listing_on_success()
        {
            // act
            Execute_Successfully();

            // assert
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Update(It.Is<ActiveListing>(activeListing => activeListing.Id == _activeListing.Id)), Times.Once);
        }

        [Fact]
        public void save_changes_on_success()
        {
            // act
            Execute_Successfully();

            // assert
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_active_listing_does_not_exist()
        {
            // act
            Execute_WithNonExistingListing();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.NotFound>();
                    error.Message.Should().Be("active listing not found");
                });
        }

        [Fact]
        public void not_persist_changes_when_active_listing_does_not_exist()
        {
            // act
            Execute_WithNonExistingListing();

            // assert
            VerifyChangesNotPersisted();
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_FavoriteMark_creation_failed()
        {
            // act
            Execute_WithFailedFavoriteMarkCreation();

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
        public void not_persist_changes_when_FavoriteMark_creation_failed()
        {
            // act
            Execute_WithFailedFavoriteMarkCreation();

            // assert
            VerifyChangesNotPersisted();
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_MarkAsFavorite_failed()
        {
            // act
            Execute_WithFailedMarkAsFavorite();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("cannot accept favorites from the listing owner");
                });
        }

        [Fact]
        public void not_persist_changes_listings_when_MarkAsFavorite_failed()
        {
            // act
            Execute_WithFailedMarkAsFavorite();

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
