using BusinessLine.Core.Application.Listings.Commands;
using BusinessLine.Core.Application.Listings.Commands.RemoveFavorite;
using BusinessLine.Core.Domain.Common;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.RemoveFavorite
{
    public class RemoveFavoriteCommand_should
    {
        private readonly RemoveFavoriteCommand _sut;
        private readonly RemoveFavoriteModel _model;
        private readonly Guid _userId = Guid.NewGuid();
        private readonly Guid _listingId = Guid.NewGuid();
        private readonly FavoriteUserListing _favorite;
        private readonly AutoMocker _mocker;

        public RemoveFavoriteCommand_should()
        {
            _mocker = new AutoMocker();
            _model = new RemoveFavoriteModel()
            {
                ListingId = _listingId
            };
            _favorite = FavoriteUserListing.Create(Guid.NewGuid(),
                Owner.Create(_userId),
                _listingId);

            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindFavorite(_userId, _listingId))
                .Returns(Option<FavoriteUserListing>.Some(_favorite));

            _sut = _mocker.CreateInstance<RemoveFavoriteCommand>();
        }

        [Fact]
        public void retrieve_favorite_listing_from_repo()
        {
            _sut.Execute(_userId, _model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.FindFavorite(_userId, _listingId), Times.Once);
        }

        [Fact]
        public void remove_favorite_listing_from_repo()
        {
            _sut.Execute(_userId, _model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Delete(It.IsAny<FavoriteUserListing>()), Times.Once);
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
        public void do_nothing_if_favorite_listing_does_not_exist()
        {
            // arrange
            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindFavorite(_userId, _listingId))
                .Returns(Option<FavoriteUserListing>.None);

            // act
            _sut.Execute(_userId, _model);

            // assert
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Delete(It.IsAny<FavoriteUserListing>()), Times.Never);
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Never);
        }
    }
}
