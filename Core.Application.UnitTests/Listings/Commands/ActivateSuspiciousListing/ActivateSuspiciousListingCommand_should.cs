using Common.Dates;
using Common.Helpers;
using Core.Application.Listings.Commands;
using Core.Application.Listings.Commands.ActivateSuspiciousListing;
using Core.Domain.Listings;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.ActivateSuspiciousListing
{
    public class ActivateSuspiciousListingCommand_should
    {
        private ActivateSuspiciousListingCommand _sut;
        private ActivateSuspiciousListingModel _model;
        private SuspiciousListing _suspiciousListing;
        private Guid _listingId = Guid.NewGuid();
        private DateTimeOffset _expirationDate = DateTimeOffset.UtcNow.AddDays(100);

        private AutoMocker _mocker;
        private Either<Error, Unit> _executionResult;

        public ActivateSuspiciousListingCommand_should()
        {
            _mocker = new AutoMocker();
            _suspiciousListing = DummyData.SuspiciousListing_1;
            _model = new ActivateSuspiciousListingModel()
            {
                ListingId = _listingId
            };

            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindSuspicious(It.IsAny<Guid>()))
                .Returns(Option<SuspiciousListing>.Some(_suspiciousListing));

            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetFutureUtcDateTime(It.IsAny<int>()))
                .Returns(_expirationDate);

            _sut = _mocker.CreateInstance<ActivateSuspiciousListingCommand>();
        }

        private void ExecuteSuccessfully()
        {
            _executionResult = _sut.Execute(_model);
        }

        private void ExecuteWithNonExistingListing()
        {
            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindSuspicious(It.IsAny<Guid>()))
                .Returns(Option<SuspiciousListing>.None);

            _executionResult = _sut.Execute(_model);
        }

        private void ExecuteWithFailedActivation()
        {
            DateTimeOffset invalidExpirationDate = default;

            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetFutureUtcDateTime(It.IsAny<int>()))
                .Returns(invalidExpirationDate);

            _executionResult = _sut.Execute(_model);
        }

        private void VerifyChangesNotPersisted()
        {
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.IsAny<ActiveListing>()), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Delete(It.IsAny<SuspiciousListing>()), Times.Never);
        }

        [Fact]
        public void return_EitherRight_on_success()
        {
            // act
            ExecuteSuccessfully();

            // assert
            _executionResult
                .Right(u => u.Should().NotBeNull())
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void delete_suspicious_listing_on_success()
        {
            // act
            ExecuteSuccessfully();

            // assert
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Delete(_suspiciousListing), Times.Once);
        }

        [Fact]
        public void add_active_listing_on_success()
        {
            // act
            ExecuteSuccessfully();

            // assert
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.Is<ActiveListing>(activeListing => activeListing.Id == _suspiciousListing.Id)), Times.Once);
        }

        [Fact]
        public void save_changes_on_success()
        {
            // act
            ExecuteSuccessfully();

            // assert
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_suspicious_listing_does_not_exist()
        {
            // act
            ExecuteWithNonExistingListing();

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
            ExecuteWithNonExistingListing();

            // assert
            VerifyChangesNotPersisted();
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_activation_failed()
        {
            // act
            ExecuteWithFailedActivation();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("expirationDate");
                });
        }

        [Fact]
        public void not_persist_changes_when_activation_failed()
        {
            // act
            ExecuteWithFailedActivation();

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
