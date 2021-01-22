using Common.Dates;
using Common.Helpers;
using Core.Application.Listings.Commands;
using Core.Application.Listings.Commands.ReactivatePassiveListing;
using Core.Domain.Listings;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.ReactivatePassiveListing
{
    public class ReactivatePassiveListingCommand_should
    {
        private ReactivatePassiveListingCommand _sut;
        private ReactivatePassiveListingModel _model;
        private PassiveListing _passiveListing;
        private Guid _listingId = Guid.NewGuid();
        private DateTimeOffset _expirationDate = DateTimeOffset.UtcNow.AddDays(23);

        private AutoMocker _mocker;
        private Either<Error, Unit> _executionResult;

        public ReactivatePassiveListingCommand_should()
        {
            _mocker = new AutoMocker();
            _passiveListing = DummyData.PassiveListing_1;
            _model = new ReactivatePassiveListingModel()
            {
                ListingId = _listingId
            };

            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindPassive(It.IsAny<Guid>()))
                .Returns(Option<PassiveListing>.Some(_passiveListing));

            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetFutureUtcDateTime(It.IsAny<int>()))
                .Returns(_expirationDate);

            _sut = _mocker.CreateInstance<ReactivatePassiveListingCommand>();
        }

        private void Execute_Successfully()
        {
            _executionResult = _sut.Execute(_model);
        }

        private void Execute_WithNonExistingListing()
        {
            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindPassive(It.IsAny<Guid>()))
                .Returns(Option<PassiveListing>.None);

            _executionResult = _sut.Execute(_model);
        }

        private void Execute_WithFailedReactivate()
        {
            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetFutureUtcDateTime(It.IsAny<int>()))
                .Returns((DateTimeOffset)default);

            _executionResult = _sut.Execute(_model);
        }

        private void VerifyChangesNotPersisted()
        {
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Delete(It.IsAny<PassiveListing>()), Times.Never);

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
                .Verify(r => r.Delete(It.IsAny<PassiveListing>()), Times.Once);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.IsAny<ActiveListing>()), Times.Once);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_passive_listing_does_not_exist()
        {
            // act
            Execute_WithNonExistingListing();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.NotFound>();
                    error.Message.Should().Be("passive listing not found");
                });
        }

        [Fact]
        public void not_persist_changes_when_passive_listing_does_not_exist()
        {
            // act
            Execute_WithNonExistingListing();

            // assert
            VerifyChangesNotPersisted();
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_reactivate_failed()
        {
            // act
            Execute_WithFailedReactivate();

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
        public void not_persist_changes_when_reactivate_failed()
        {
            // act
            Execute_WithFailedReactivate();

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
