using Common.Dates;
using Core.Application.Listings.Commands;
using Core.Application.Listings.Commands.MarkNewListingAsSuspicious;
using Core.Domain.Listings;
using Core.UnitTests.Mocks;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.MarkNewListingAsSuspicious
{
    public class MarkNewListingAsSuspicious_should
    {
        private readonly MarkNewListingAsSuspiciousCommand _sut;
        private readonly MarkNewListingAsSuspiciousModel _model;
        private readonly NewListing _newListing;
        private readonly AutoMocker _mocker;
        private readonly Guid _listingId = Guid.NewGuid();
        private readonly DateTimeOffset _markedAsSuspiciousAtDate = DateTimeOffset.UtcNow.AddDays(1);

        public MarkNewListingAsSuspicious_should()
        {
            _mocker = new AutoMocker();
            _newListing = FakesCollection.NewListing_1;
            _model = new MarkNewListingAsSuspiciousModel()
            {
                ListingId = _listingId,
                Reason = "aaaaa"
            };

            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindNew(It.IsAny<Guid>()))
                .Returns(Option<NewListing>.Some(_newListing));

            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetCurrentUtcDateTime())
                .Returns(_markedAsSuspiciousAtDate);

            _sut = _mocker.CreateInstance<MarkNewListingAsSuspiciousCommand>();
        }

        [Fact(Skip = "while refactoring")]
        public void retrieve_the_new_listing_from_repository()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.FindNew(_listingId), Times.Once);
        }

        [Fact(Skip = "while refactoring")]
        public void add_suspicious_listing_to_the_repo()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.Is<SuspiciousListing>(l => l.Id == _newListing.Id)), Times.Once);
        }

        [Fact(Skip = "while refactoring")]
        public void remove_the_new_listing_from_the_repo()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Delete(_newListing), Times.Once);
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
                .Setup(r => r.FindNew(It.IsAny<Guid>()))
                .Returns(Option<NewListing>.None);

            // act
            _sut.Execute(_model);

            // assert
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Delete(It.IsAny<NewListing>()), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(It.IsAny<SuspiciousListing>()), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Never);
        }
    }
}
