using Core.Application.Listings.Commands;
using Core.Application.Listings.Commands.ReceiveOffer;
using Core.Application.Listings.Commands.ReceiveOffer.Factory;
using BusinessLine.Core.Application.UnitTests.TestMocks;
using Core.Domain.Common;
using Core.Domain.Listings;
using Core.Domain.Offers;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.ReceiveOffer
{
    public class ReceiveOfferCommand_should
    {
        private readonly ReceiveOfferCommand _sut;
        private readonly ReceiveOfferModel _model;
        private readonly AutoMocker _mocker;
        private readonly ActiveListing _activeListing;
        private readonly ReceivedOffer _offer;
        private readonly Guid _listingId = Guid.NewGuid();
        private readonly Guid _userId = Guid.NewGuid();

        public ReceiveOfferCommand_should()
        {
            _mocker = new AutoMocker();
            _model = new ReceiveOfferModel()
            {
                ListingId = _listingId,
                Value = 2.5M,
                CurrencyCode = "USD"
            };
            _activeListing = ListingMocks.ActiveListing_1;
            _offer = ListingMocks.Offer_1;

            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindActive(_listingId))
                .Returns(Option<ActiveListing>.Some(_activeListing));

            _mocker
                .GetMock<IOfferFactory>()
                .Setup(f => f.Create(It.IsAny<Owner>(), It.IsAny<MonetaryValue>()))
                .Returns(_offer);


            _sut = _mocker.CreateInstance<ReceiveOfferCommand>();
        }

        [Fact]
        public void retrieve_active_listing_from_repo()
        {
            _sut.Execute(_userId, _model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.FindActive(_listingId), Times.Once);
        }


        [Fact]
        public void crete_offer()
        {
            _sut.Execute(_userId, _model);

            _mocker
                .GetMock<IOfferFactory>()
                .Verify(r => r.Create(It.IsAny<Owner>(), It.IsAny<MonetaryValue>()), Times.Once);
        }

        [Fact]
        public void save_changes_to_repository()
        {
            _sut.Execute(_userId, _model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void do_nothing_if_listing_is_none()
        {
            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindActive(_listingId))
                .Returns(Option<ActiveListing>.None);

            _sut.Execute(_userId, _model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Never);
        }
    }
}
