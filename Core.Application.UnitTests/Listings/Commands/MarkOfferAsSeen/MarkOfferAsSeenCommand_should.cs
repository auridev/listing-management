using Common.Dates;
using Core.Application.Listings.Commands;
using Core.Application.Listings.Commands.MarkOfferAsSeen;
using Core.Domain.Listings;
using Core.UnitTests.Mocks;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.MarkOfferAsSeen
{
    public class MarkOfferAsSeenCommand_should
    {
        private readonly MarkOfferAsSeenCommand _sut;
        private readonly MarkOfferAsSeenModel _model;
        private readonly ActiveListing _activeListing;
        private readonly AutoMocker _mocker;
        private readonly Guid _offerId = Guid.NewGuid();
        private readonly Guid _listingId = Guid.NewGuid();
        private readonly DateTimeOffset _seenDate = DateTimeOffset.UtcNow.AddDays(-1);

        public MarkOfferAsSeenCommand_should()
        {
            _mocker = new AutoMocker();
            _activeListing = FakesCollection.ActiveListing_2;
            _model = new MarkOfferAsSeenModel()
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
                .Returns(_seenDate);

            _sut = _mocker.CreateInstance<MarkOfferAsSeenCommand>();
        }

        [Fact]
        public void retrieve_listing_from_repository()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.FindActive(_listingId), Times.Once);
        }

        [Fact]
        public void update_the_listing()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Update(It.IsAny<ActiveListing>()), Times.Once);
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
        public void do_nothing_if_listing_is_not_found()
        {
            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindActive(It.IsAny<Guid>()))
                .Returns(Option<ActiveListing>.None);

            _sut.Execute(_model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Update(It.IsAny<ActiveListing>()), Times.Never);
        }
    }
}
