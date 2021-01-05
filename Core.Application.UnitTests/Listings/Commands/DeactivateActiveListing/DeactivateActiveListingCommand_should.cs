using Common.Dates;
using Core.Application.Listings.Commands;
using Core.Application.Listings.Commands.DeactivateActiveListing;
using Core.Domain.Listings;
using Core.UnitTests.Mocks;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.DeactivateActiveListing
{
    public class DeactivateActiveListingCommand_should
    {
        private readonly DeactivateActiveListingCommand _sut;
        private readonly DeactivateActiveListingModel _model;
        private readonly ActiveListing _activeListing;
        private readonly Guid _listingId = Guid.NewGuid();
        private readonly AutoMocker _mocker;

        public DeactivateActiveListingCommand_should()
        {
            _mocker = new AutoMocker();
            _activeListing = FakesCollection.ActiveListing_1;
            _model = new DeactivateActiveListingModel()
            {
                ListingId = _listingId,
                Reason = "not fun"
            };

            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetCurrentUtcDateTime())
                .Returns(DateTimeOffset.UtcNow);

            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindActive(_listingId))
                .Returns(Option<ActiveListing>.Some(_activeListing));

            _sut = _mocker.CreateInstance<DeactivateActiveListingCommand>();
        }


        [Fact(Skip = "while refactoring")]
        public void retrieve_listing_from_repository()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.FindActive(_listingId), Times.Once);
        }

        [Fact(Skip = "while refactoring")]
        public void add_passive_listing_to_the_repo()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.Is<PassiveListing>(l => l.Id == _activeListing.Id)), Times.Once);
        }

        [Fact(Skip = "while refactoring")]
        public void remove_active_listing_from_the_repo()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Delete(_activeListing), Times.Once);
        }

        [Fact(Skip = "while refactoring")]
        public void save_repository_changes()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Once);
        }

        [Fact(Skip = "while refactoring")]
        public void do_nothing_if_listing_is_not_found()
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
                .Verify(r => r.Add(It.IsAny<PassiveListing>()), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Delete(It.IsAny<ActiveListing>()), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Never);
        }
    }
}
