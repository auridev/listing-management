using Common.Helpers;
using Core.Application.Listings.Commands;
using Core.Application.Listings.Commands.ReceiveOffer;
using Core.Application.Listings.Commands.ReceiveOffer.Factory;
using Core.Domain.Listings;
using Core.Domain.Offers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Test.Helpers;
using Xunit;
using static LanguageExt.Prelude;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.ReceiveOffer
{
    public class ReceiveOfferCommand_should
    {
        private ReceiveOfferCommand _sut;
        private ReceiveOfferModel _model;
        private ActiveListing _activeListing;
        private ReceivedOffer _offer;
        private Guid _listingId = Guid.NewGuid();
        private Guid _userId = Guid.NewGuid();

        private AutoMocker _mocker;
        private Either<Error, Unit> _executionResult;

        public ReceiveOfferCommand_should()
        {
            _mocker = new AutoMocker();
            _model = new ReceiveOfferModel()
            {
                ListingId = _listingId,
                Value = 2.5M,
                CurrencyCode = "USD"
            };
            _activeListing = DummyData.ActiveListing_1;
            _offer = DummyData.Offer_1;

            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindActive(It.IsAny<Guid>()))
                .Returns(Option<ActiveListing>.Some(_activeListing));

            _mocker
                .GetMock<IOfferFactory>()
                .Setup(f => f.Create(It.IsAny<Owner>(), It.IsAny<MonetaryValue>()))
                .Returns(_offer);


            _sut = _mocker.CreateInstance<ReceiveOfferCommand>();
        }

        private void ExecuteWith_Success()
        {
            _executionResult = _sut.Execute(_userId, _model);
        }

        private void ExecuteWith_ListingNotFound()
        {
            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindActive(It.IsAny<Guid>()))
                .Returns(Option<ActiveListing>.None);

            _executionResult = _sut.Execute(_userId, _model);
        }

        private void ExecuteWith_FailedOwner()
        {
            _executionResult = _sut.Execute(default, _model);
        }

        private void ExecuteWith_FailedMonetaryValue()
        {
            _executionResult = _sut.Execute(_userId, new ReceiveOfferModel()
            {
                ListingId = _listingId,
                Value = 0.0M,
                CurrencyCode = "USD"
            });
        }

        private void ExecuteWith_FailedOffer()
        {
            _mocker
                .GetMock<IOfferFactory>()
                .Setup(f => f.Create(It.IsAny<Owner>(), It.IsAny<MonetaryValue>()))
                .Returns(Left<Error, ReceivedOffer>(new Error.Invalid("some invalid offer")));

            _executionResult = _sut.Execute(_userId, _model);
        }

        private void ExecuteWith_FailedReceiveOffer()
        {
            var listingOwner = Owner.Create(_activeListing.Owner.UserId)
                .Right(value => value)
                .Left(_ => throw InvalidExecutionPath.Exception);
            var monetaryValue = MonetaryValue.Create(1.0M, "EUR")
                .Right(value => value)
                .Left(_ => throw InvalidExecutionPath.Exception);
            var invalidOffer = new ReceivedOffer(
                Guid.NewGuid(), 
                listingOwner, 
                monetaryValue, 
                DateTimeOffset.UtcNow);
            _mocker
               .GetMock<IOfferFactory>()
               .Setup(f => f.Create(It.IsAny<Owner>(), It.IsAny<MonetaryValue>()))
               .Returns(Right<Error, ReceivedOffer>(invalidOffer));

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
                .Verify(r => r.Save(), Times.Once);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Update(It.IsAny<ActiveListing>()), Times.Once);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_active_listing_does_not_exist()
        {
            // act
            ExecuteWith_ListingNotFound();

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
            ExecuteWith_ListingNotFound();

            // assert
            VerifyChangesNotPersisted();
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

        [Fact]
        public void return_EitherLeft_with_proper_error_when_monetary_value_creation_failed()
        {
            // act
            ExecuteWith_FailedMonetaryValue();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("invalid monetary value");
                });
        }

        [Fact]
        public void not_persist_changes_when_monetary_value_creation_failed()
        {
            // act
            ExecuteWith_FailedMonetaryValue();

            // assert
            VerifyChangesNotPersisted();
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_offer_creation_failed()
        {
            // act
            ExecuteWith_FailedOffer();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("some invalid offer");
                });
        }

        [Fact]
        public void not_persist_changes_when_offer_creation_failed()
        {
            // act
            ExecuteWith_FailedOffer();

            // assert
            VerifyChangesNotPersisted();
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_ReceiveOffer_failed()
        {
            // act
            ExecuteWith_FailedReceiveOffer();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("cannot accept offers from the listing owner");
                });
        }

        [Fact]
        public void not_persist_changes_when_ReceiveOffer_failed()
        {
            // act
            ExecuteWith_FailedReceiveOffer();

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
