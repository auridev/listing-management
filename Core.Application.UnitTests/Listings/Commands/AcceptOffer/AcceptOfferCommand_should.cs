using Common.Dates;
using Core.Application.Listings.Commands;
using Core.Application.Listings.Commands.AcceptOffer;
using Core.Domain.Listings;
using Core.UnitTests.Mocks;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.AcceptOffer
{
    public class AcceptOfferCommand_should
    {
        private readonly AcceptOfferCommand _sut;
        private readonly AcceptOfferModel _model;
        private readonly AutoMocker _mocker;
        private readonly ActiveListing _activeListing;
        private readonly Guid _listingId;
        private readonly Guid _offerId;

        public AcceptOfferCommand_should()
        {
            _mocker = new AutoMocker();
            _activeListing = FakesCollection.ActiveListing_2;
            _activeListing.ReceiveOffer(FakesCollection.Offer_1);
            _activeListing.ReceiveOffer(FakesCollection.Offer_2);
            _listingId = _activeListing.Id;
            _offerId = FakesCollection.Offer_1.Id;

            _model = new AcceptOfferModel
            {
                ListingId = _listingId,
                OfferId = _offerId
            };

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

        [Fact]
        public void retrieve_active_listing_from_repo()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.FindActive(_listingId), Times.Once);
        }

        [Fact]
        public void add_closed_listing_to_the_repo()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.Is<ClosedListing>(l => l.Id == _activeListing.Id)), Times.Once);
        }

        [Fact]
        public void remove_active_listing_from_the_repo()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Delete(_activeListing), Times.Once);
        }

        [Fact]
        public void save_repository_changes()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void do_nothing_if_listing_is_none()
        {
            // arrange
            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindActive(_listingId))
                .Returns(Option<ActiveListing>.None);

            // act
            _sut.Execute(_model);

            // assert
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Delete(It.IsAny<ActiveListing>()), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.IsAny<ClosedListing>()), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Never);
        }
    }
}
