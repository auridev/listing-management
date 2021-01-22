using Common.Dates;
using Common.Helpers;
using Core.Application.Listings.Commands;
using Core.Application.Listings.Commands.AcceptOffer;
using Core.Domain.Listings;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Test.Helpers;
using Xunit;

namespace Core.Application.UnitTests.Listings.Commands.AcceptOffer
{
    public class AcceptOfferCommand_should
    {
        private AcceptOfferCommand _sut;
        private ActiveListing _activeListing;
        private Guid _listingId;
        private Guid _offerId;

        private AutoMocker _mocker;
        private Either<Error, Unit> _executionResult;

        public AcceptOfferCommand_should()
        {
            _mocker = new AutoMocker();
            _activeListing = DummyData.ActiveListing_2;
            _activeListing.ReceiveOffer(DummyData.Offer_1);
            _activeListing.ReceiveOffer(DummyData.Offer_2);
            _listingId = _activeListing.Id;
            _offerId = DummyData.Offer_1.Id;

            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindActive(_listingId))
                .Returns(Option<ActiveListing>.Some(_activeListing));

            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetCurrentUtcDateTime())
                .Returns(DateTimeOffset.UtcNow);

            _sut = _mocker.CreateInstance<AcceptOfferCommand>();
        }

        private void VerifyChangesNotPersisted()
        {
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Delete(_activeListing), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.IsAny<ClosedListing>()), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Never);
        }

        private void ExecuteSuccessfully()
        {
            _executionResult = _sut.Execute(new AcceptOfferModel
            {
                ListingId = _listingId,
                OfferId = _offerId
            });
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
                .Verify(r => r.Delete(_activeListing), Times.Once);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.IsAny<ClosedListing>()), Times.Once);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Once);
        }

        private void ExecuteWithNonExistingListing()
        {
            _executionResult = _sut.Execute(new AcceptOfferModel
            {
                ListingId = Guid.NewGuid(),
                OfferId = _offerId
            });
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_active_listing_does_not_exist()
        {
            // act
            ExecuteWithNonExistingListing();

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
            ExecuteWithNonExistingListing();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWithRejectedOffer()
        {
            _executionResult = _sut.Execute(new AcceptOfferModel
            {
                ListingId = _listingId,
                OfferId = Guid.NewGuid()
            });
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_offer_has_not_been_accepted()
        {
            // act
            ExecuteWithRejectedOffer();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.NotFound>();
                    error.Message.Should().Be("offer not found");
                });
        }

        [Fact]
        public void not_persist_changes_when_offer_has_not_been_accepted()
        {
            // act
            ExecuteWithRejectedOffer();

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
