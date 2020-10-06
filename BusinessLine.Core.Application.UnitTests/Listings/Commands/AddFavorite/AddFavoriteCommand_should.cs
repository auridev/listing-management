using BusinessLine.Core.Application.Listings.Commands;
using BusinessLine.Core.Application.Listings.Commands.AddFavorite;
using BusinessLine.Core.Application.UnitTests.TestMocks;
using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Listings;
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
            _activeListing = ListingMocks.ActiveListing_1;

            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindActive(_listingId))
                .Returns(Option<ActiveListing>.Some(_activeListing));

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
        public void add_favorite_listing_to_repo()
        {
            _sut.Execute(_userId, _model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.IsAny<FavoriteUserListing>()), Times.Once);
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
            CheckMethodsWerentCalled();
        }

        [Fact]
        public void do_nothing_if_listing_and_favorite_listing_have_same_owner() // i.e. someone's marking their own listings
        {
            var sameUserId = Guid.Parse("a976bed2-0340-4408-b501-5334d509f11e");

            _sut.Execute(sameUserId, _model);

            // assert
            CheckMethodsWerentCalled();
        }

        private void CheckMethodsWerentCalled()
        {
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.IsAny<FavoriteUserListing>()), Times.Never);
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Never);
        }
    }
}
