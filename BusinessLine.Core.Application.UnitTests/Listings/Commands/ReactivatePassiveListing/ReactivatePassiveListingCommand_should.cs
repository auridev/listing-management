using Core.Application.Listings.Commands;
using Core.Application.Listings.Commands.ReactivatePassiveListing;
using BusinessLine.Core.Application.UnitTests.TestMocks;
using Core.Domain.Listings;
using Common.Dates;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;


namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.ReactivatePassiveListing
{
    public class ReactivatePassiveListingCommand_should
    {
        private readonly ReactivatePassiveListingCommand _sut;
        private readonly ReactivatePassiveListingModel _model;
        private readonly PassiveListing _passiveListing;
        private readonly AutoMocker _mocker;
        private readonly Guid _listingId = Guid.NewGuid();
        private readonly DateTimeOffset _expirationDate = DateTimeOffset.UtcNow.AddDays(23);


        public ReactivatePassiveListingCommand_should()
        {
            _mocker = new AutoMocker();
            _passiveListing = ListingMocks.PassiveListing_1;
            _model = new ReactivatePassiveListingModel()
            {
                ListingId = _listingId
            };

            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindPassive(It.IsAny<Guid>()))
                .Returns(Option<PassiveListing>.Some(_passiveListing));

            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetFutureUtcDateTime(It.IsAny<int>()))
                .Returns(_expirationDate);

            _sut = _mocker.CreateInstance<ReactivatePassiveListingCommand>();
        }

        [Fact]
        public void retrieve_listing_from_repository()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.FindPassive(_listingId), Times.Once);
        }

        [Fact]
        public void add_active_listing_to_the_repo()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.Is<ActiveListing>(l => l.Id == _passiveListing.Id)), Times.Once);
        }

        [Fact]
        public void remove_suspicious_listing_from_the_repo()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Delete(_passiveListing), Times.Once);
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
                .Setup(r => r.FindPassive(It.IsAny<Guid>()))
                .Returns(Option<PassiveListing>.None);

            // act
            _sut.Execute(_model);

            // assert
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Delete(_passiveListing), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.IsNotNull<ActiveListing>()), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Never);
        }
    }
}
