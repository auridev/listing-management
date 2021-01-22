using Common.Dates;
using Common.Helpers;
using Core.Application.Listings.Commands;
using Core.Application.Listings.Commands.ActivateNewListing;
using Core.Domain.Listings;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.ActivateNewListing
{
    public class ActivateNewListingCommand_should
    {
        private ActivateNewListingCommand _sut;
        private NewListing _newListing;
        private ActivateNewListingModel _model;
        private Guid _listingId = Guid.NewGuid();
        private DateTimeOffset _expirationDate = DateTimeOffset.UtcNow.AddDays(1);

        private AutoMocker _mocker;
        private Either<Error, Unit> _executionResult;

        public ActivateNewListingCommand_should()
        {
            _mocker = new AutoMocker();
            _newListing = DummyData.NewListing_1;
            _model = new ActivateNewListingModel()
            {
                ListingId = _listingId
            };

            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindNew(It.IsAny<Guid>()))
                .Returns(Option<NewListing>.Some(_newListing));

            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetFutureUtcDateTime(It.IsAny<int>()))
                .Returns(_expirationDate);

            _sut = _mocker.CreateInstance<ActivateNewListingCommand>();
        }

        private void VerifyChangesNotPersisted()
        {
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Delete(_newListing), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.IsAny<ActiveListing>()), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Never);
        }

        private void ExecuteSuccessfully()
        {
            _executionResult = _sut.Execute(_model);
        }

        private void ExecuteWithNonExistingListing()
        {
            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindNew(It.IsAny<Guid>()))
                .Returns(Option<NewListing>.None);

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
        public void persist_changes_on_success()
        {
            // act
            ExecuteSuccessfully();

            // assert
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Delete(_newListing), Times.Once);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.IsAny<ActiveListing>()), Times.Once);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_new_listing_does_not_exist()
        {
            // act
            ExecuteWithNonExistingListing();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.NotFound>();
                    error.Message.Should().Be("new listing not found");
                });
        }

        [Fact]
        public void not_persist_changes_when_new_listing_does_not_exist()
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
