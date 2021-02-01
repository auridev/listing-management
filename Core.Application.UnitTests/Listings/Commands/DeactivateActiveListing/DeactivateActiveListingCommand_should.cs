using Common.Dates;
using Common.Helpers;
using Core.Application.Listings.Commands;
using Core.Application.Listings.Commands.DeactivateActiveListing;
using Core.Domain.Listings;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.DeactivateActiveListing
{
    public class DeactivateActiveListingCommand_should
    {
        private DeactivateActiveListingCommand _sut;
        private DeactivateActiveListingModel _model;
        private ActiveListing _activeListing;
        private Guid _listingId = Guid.NewGuid();

        private AutoMocker _mocker;
        private Either<Error, Unit> _executionResult;

        public DeactivateActiveListingCommand_should()
        {
            _mocker = new AutoMocker();
            _activeListing = DummyData.ActiveListing_1;
            _model = new DeactivateActiveListingModel()
            {
                ListingId = _listingId,
                Reason = "not fun"
            };

            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindActive(It.IsAny<Guid>()))
                .Returns(Option<ActiveListing>.Some(_activeListing));

            _sut = _mocker.CreateInstance<DeactivateActiveListingCommand>();
        }

        private void Execute_Successfully()
        {
            _executionResult = _sut.Execute(_model);
        }

        private void Execute_WithNonExistingListing()
        {
            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindActive(It.IsAny<Guid>()))
                .Returns(Option<ActiveListing>.None);

            _executionResult = _sut.Execute(_model);
        }

        private void Execute_WithFailedDeacticationReasonCreation()
        {
            _executionResult = _sut.Execute(new DeactivateActiveListingModel()
            {
                ListingId = _listingId,
                Reason = string.Empty
            });
        }

        private void VerifyChangesNotPersisted()
        {
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.IsAny<PassiveListing>()), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Delete(It.IsAny<ActiveListing>()), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Never);
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
                .Verify(r => r.Delete(It.IsAny<ActiveListing>()), Times.Once);

            _mocker
                 .GetMock<IListingRepository>()
                 .Verify(r => r.Add(It.IsAny<PassiveListing>()), Times.Once);

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
        public void return_EitherLeft_with_proper_error_when_deactivation_reason_creation_failed()
        {
            // act
            Execute_WithFailedDeacticationReasonCreation();

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
        public void not_persist_changes_when_deactivation_reason_creation_failed()
        {
            // act
            Execute_WithFailedDeacticationReasonCreation();

            // assert
            VerifyChangesNotPersisted();
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
    }
}
