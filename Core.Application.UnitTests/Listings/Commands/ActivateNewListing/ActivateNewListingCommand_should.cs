using Core.Application.Listings.Commands;
using Core.Application.Listings.Commands.ActivateNewListing;
using BusinessLine.Core.Application.UnitTests.TestMocks;
using Core.Domain.Listings;
using Common.Dates;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.ActivateNewListing
{
    public class ActivateNewListingCommand_should
    {
        private readonly ActivateNewListingCommand _sut;
        private readonly ActivateNewListingModel _model;
        private readonly NewListing _newListing;
        private readonly AutoMocker _mocker;
        private readonly Guid _listingId = Guid.NewGuid();
        private readonly DateTimeOffset _expirationDate = DateTimeOffset.UtcNow.AddDays(1);

        public ActivateNewListingCommand_should()
        {
            _mocker = new AutoMocker();
            _newListing = ListingMocks.NewListing_1;
            _model = new ActivateNewListingModel()
            {
                ListingId = _listingId
            };

            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindNew(It.IsAny<Guid>()))
                .Returns(Option<NewListing>.Some(_newListing));

            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetFutureUtcDateTime(It.IsAny<int>()))
                .Returns(_expirationDate);

            _sut = _mocker.CreateInstance<ActivateNewListingCommand>();
        }

        [Fact]
        public void retrieve_new_listing_from_repository()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.FindNew(_listingId), Times.Once);
        }

        [Fact]
        public void add_active_listing_to_the_repo()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.IsNotNull<ActiveListing>()), Times.Once);
        }

        [Fact]
        public void add_active_listing_with_correc_exipration_date_to_the_repo()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.Is<ActiveListing>(p => p.ExpirationDate.Equals(_expirationDate))), Times.Once);
        }

        [Fact]
        public void remove_the_new_listing_from_the_repo()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Delete(_newListing), Times.Once);
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
            // arrange
            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindNew(It.IsAny<Guid>()))
                .Returns(Option<NewListing>.None);

            // act
            _sut.Execute(_model);

            // assert
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Delete(_newListing), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.IsNotNull<ActiveListing>()), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Never);
        }
    }
}
