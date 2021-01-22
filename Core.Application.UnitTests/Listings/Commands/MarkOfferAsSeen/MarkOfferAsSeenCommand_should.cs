using Common.Dates;
using Common.Helpers;
using Core.Application.Listings.Commands;
using Core.Application.Listings.Commands.MarkOfferAsSeen;
using Core.Domain.Listings;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.MarkOfferAsSeen
{
    public class MarkOfferAsSeenCommand_should
    {
        private MarkOfferAsSeenCommand _sut;
        private MarkOfferAsSeenModel _model;
        private ActiveListing _activeListing;
        private Guid _listingId = Guid.NewGuid();
        private DateTimeOffset _seenDate = DateTimeOffset.UtcNow.AddDays(-1);

        private AutoMocker _mocker;
        private Either<Error, Unit> _executionResult;

        public MarkOfferAsSeenCommand_should()
        {
            _mocker = new AutoMocker();
            _activeListing = DummyData.ActiveListing_2;
            _activeListing.ReceiveOffer(DummyData.Offer_1); // we need to add some offers for this to work
            _activeListing.ReceiveOffer(DummyData.Offer_2);
            _model = new MarkOfferAsSeenModel()
            {
                ListingId = _listingId,
                OfferId = DummyData.Offer_1.Id
            };

            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindActive(It.IsAny<Guid>()))
                .Returns(Option<ActiveListing>.Some(_activeListing));

            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetCurrentUtcDateTime())
                .Returns(_seenDate);

            _sut = _mocker.CreateInstance<MarkOfferAsSeenCommand>();
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

        private void Execute_WithFailedSeenDateCreation()
        {
            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetCurrentUtcDateTime())
                .Returns((DateTimeOffset)default);

            _executionResult = _sut.Execute(_model);
        }

        private void Execute_WithFailedMarkOfferAsSeen()
        {
            _executionResult = _sut.Execute(new MarkOfferAsSeenModel()
            {
                ListingId = _listingId,
                OfferId = default
            });
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
        public void return_EitherLeft_with_proper_error_when_seen_date_creation_failed()
        {
            // act
            Execute_WithFailedSeenDateCreation();

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
        public void not_persist_changes_when_seen_date_creation_failed()
        {
            // act
            Execute_WithFailedSeenDateCreation();

            // assert
            VerifyChangesNotPersisted();
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_MarkOfferAsSeen_failed()
        {
            // act
            Execute_WithFailedMarkOfferAsSeen();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("offerId");
                });
        }

        [Fact]
        public void not_persist_changes_when_MarkOfferAsSeen_failed()
        {
            // act
            Execute_WithFailedMarkOfferAsSeen();

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
