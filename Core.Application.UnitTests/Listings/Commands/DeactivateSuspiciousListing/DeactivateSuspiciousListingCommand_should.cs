using Common.Dates;
using Common.Helpers;
using Core.Application.Listings.Commands;
using Core.Application.Listings.Commands.DeactivateSuspiciousListing;
using Core.Domain.Listings;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.DeactivateSuspiciousListing
{
    public class DeactivateSuspiciousListingCommand_should
    {
        private DeactivateSuspiciousListingCommand _sut;
        private DeactivateSuspiciousListingModel _model;
        private SuspiciousListing _suspiciousListing;
        private Guid _listingId = Guid.NewGuid();
        private DateTimeOffset _deactivationDate = DateTimeOffset.UtcNow.AddDays(1);

        private AutoMocker _mocker;
        private Either<Error, Unit> _executionResult;

        public DeactivateSuspiciousListingCommand_should()
        {
            _mocker = new AutoMocker();
            _suspiciousListing = DummyData.SuspiciousListing_1;
            _model = new DeactivateSuspiciousListingModel()
            {
                ListingId = _listingId,
                Reason = "some random reason"
            };

            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindSuspicious(It.IsAny<Guid>()))
                .Returns(Option<SuspiciousListing>.Some(_suspiciousListing));

            _sut = _mocker.CreateInstance<DeactivateSuspiciousListingCommand>();
        }

        private void Execute_Successfully()
        {
            _executionResult = _sut.Execute(_model);
        }

        private void Execute_WithNonExistingListing()
        {
            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindSuspicious(It.IsAny<Guid>()))
                .Returns(Option<SuspiciousListing>.None);

            _executionResult = _sut.Execute(_model);
        }

        private void Execute_WithFailedDeacticationReasonCreation()
        {
            _executionResult = _sut.Execute(new DeactivateSuspiciousListingModel()
            {
                ListingId = _listingId,
                Reason = string.Empty
            });
        }

        private void VerifyChangesNotPersisted()
        {
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Delete(It.IsAny<SuspiciousListing>()), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.IsAny<ActiveListing>()), Times.Never);

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
                .Verify(r => r.Delete(It.IsAny<SuspiciousListing>()), Times.Once);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.IsAny<PassiveListing>()), Times.Once);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_suspicious_listing_does_not_exist()
        {
            // act
            Execute_WithNonExistingListing();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.NotFound>();
                    error.Message.Should().Be("suspicious listing not found");
                });
        }

        [Fact]
        public void not_persist_changes_when_suspicious_listing_does_not_exist()
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
