using BusinessLine.Common.Dates;
using BusinessLine.Core.Application.Listings.Commands;
using BusinessLine.Core.Application.Listings.Commands.DeactivateSuspiciousListing;
using BusinessLine.Core.Application.UnitTests.TestMocks;
using BusinessLine.Core.Domain.Listings;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.DeactivateSuspiciousListing
{
    public class DeactivateSuspiciousListingCommand_should
    {
        private readonly DeactivateSuspiciousListingCommand _sut;
        private readonly DeactivateSuspiciousListingModel _model;
        private readonly SuspiciousListing _suspiciousListing;
        private readonly AutoMocker _mocker;
        private readonly Guid _listingId = Guid.NewGuid();
        private readonly DateTimeOffset _deactivationDate = DateTimeOffset.UtcNow.AddDays(1);

        public DeactivateSuspiciousListingCommand_should()
        {
            _mocker = new AutoMocker();
            _suspiciousListing = ListingMocks.SuspiciousListing_1;
            _model = new DeactivateSuspiciousListingModel()
            {
                ListingId = _listingId,
                Reason = "some random reason"
            };

            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindSuspicious(It.IsAny<Guid>()))
                .Returns(Option<SuspiciousListing>.Some(_suspiciousListing));

            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetCurrentUtcDateTime())
                .Returns(_deactivationDate);

            _sut = _mocker.CreateInstance<DeactivateSuspiciousListingCommand>();
        }

        [Fact]
        public void retrieve_the_new_listing_from_repository()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.FindSuspicious(_listingId), Times.Once);
        }

        [Fact]
        public void add_passive_listing_to_the_repo()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.Is<PassiveListing>(l => l.Id == _suspiciousListing.Id)), Times.Once);
        }

        [Fact]
        public void remove_suspicious_listing_from_the_repo()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Delete(_suspiciousListing), Times.Once);
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
                .Setup(r => r.FindSuspicious(It.IsAny<Guid>()))
                .Returns(Option<SuspiciousListing>.None);

            // act
            _sut.Execute(_model);

            // assert
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Delete(It.IsAny<SuspiciousListing>()), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.IsAny<ActiveListing>()), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Never);
        }
    }
}
