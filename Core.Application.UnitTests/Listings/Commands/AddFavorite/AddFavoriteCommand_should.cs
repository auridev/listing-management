using Common.Dates;
using Core.Application.Listings.Commands;
using Core.Application.Listings.Commands.AddFavorite;
using Core.Domain.Listings;
using Core.UnitTests.Mocks;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.AddFavorite
{
    public class AddFavoriteCommand_should
    {
        private readonly AddFavoriteCommand _sut;
        private readonly AddFavoriteModel _model;
        private readonly Guid _userId = Guid.NewGuid();
        private readonly Guid _listingId = Guid.NewGuid();
        private readonly ActiveListing _activeListing;
        private readonly AutoMocker _mocker;

        public AddFavoriteCommand_should()
        {
            _mocker = new AutoMocker();
            _model = new AddFavoriteModel()
            {
                ListingId = _listingId
            };
            _activeListing = FakesCollection.ActiveListing_1;

            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindActive(_listingId))
                .Returns(Option<ActiveListing>.Some(_activeListing));

            _mocker
                .GetMock<IDateTimeService>()
                .Setup(r => r.GetCurrentUtcDateTime())
                .Returns(DateTimeOffset.UtcNow);

            _sut = _mocker.CreateInstance<AddFavoriteCommand>();
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
        public void update_the_listing()
        {
            _sut.Execute(_userId, _model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Update(It.IsAny<ActiveListing>()), Times.Once);
        }

        [Fact]
        public void save_changes_to_repo()
        {
            _sut.Execute(_userId, _model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void do_nothing_if_active_listing_does_not_exist()
        {
            // arrange
            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindActive(_listingId))
                .Returns(Option<ActiveListing>.None);

            // act
            _sut.Execute(_userId, _model);

            // assert
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Update(It.IsAny<ActiveListing>()), Times.Never);
        }
    }
}
