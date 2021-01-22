using Common.Helpers;
using Core.Application.Listings.Commands;
using Core.Application.Listings.Commands.RemoveFavorite;
using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.RemoveFavorite
{
    public class RemoveFavoriteCommand_should
    {
        private RemoveFavoriteCommand _sut;
        private RemoveFavoriteModel _model;
        private Guid _userId = Guid.NewGuid();
        private Guid _listingId = Guid.NewGuid();
        private ActiveListing _activeListing;

        private AutoMocker _mocker;
        private Either<Error, Unit> _executionResult;

        public RemoveFavoriteCommand_should()
        {
            _mocker = new AutoMocker();
            _model = new RemoveFavoriteModel()
            {
                ListingId = _listingId
            };

            _activeListing = DummyData.ActiveListing_1;

            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindActive(It.IsAny<Guid>()))
                .Returns(Option<ActiveListing>.Some(_activeListing));

            _sut = _mocker.CreateInstance<RemoveFavoriteCommand>();
        }

        private void Execute_Successfully()
        {
            // add a FavoriteMark so it can be removed
            FavoriteMark favoriteMark =
                FavoriteMark
                    .Create(_userId, DateTimeOffset.UtcNow)
                    .Right(value => value)
                    .Left(_ => throw InvalidExecutionPath.Exception);

            _activeListing.MarkAsFavorite(favoriteMark);

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

        private void Execute_WithFailedOwnerCreation()
        {
            _executionResult = _sut.Execute(default, _model);
        }

        private void Execute_WithFailedRemoveFavorite()
        {
            // there's no favorite marks so RemoveFavorite will fail by default
            _executionResult = _sut.Execute(_userId, _model);
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
        public void persist_changes_on_success()
        {
            // act
            Execute_Successfully();

            // assert
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Once);
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Update(It.IsAny<ActiveListing>()), Times.Once);
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
        public void return_EitherLeft_with_proper_error_when_owner_creation_failed()
        {
            // act
            Execute_WithFailedOwnerCreation();

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
            Execute_WithFailedOwnerCreation();

            // assert
            VerifyChangesNotPersisted();
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_RemoveFavorite_failed()
        {
            // act
            Execute_WithFailedRemoveFavorite();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.NotFound>();
                    error.Message.Should().Be("favoredBy");
                });
        }

        [Fact]
        public void not_persist_changes_when_RemoveFavorite_failed()
        {
            // act
            Execute_WithFailedRemoveFavorite();

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
