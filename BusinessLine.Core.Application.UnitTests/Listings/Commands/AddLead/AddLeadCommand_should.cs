using Core.Application.Listings.Commands;
using Core.Application.Listings.Commands.AddLead;
using BusinessLine.Core.Application.UnitTests.TestMocks;
using Core.Domain.Listings;
using Common.Dates;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.AddLead
{
    public class AddLeadCommand_should
    {
        private readonly AddLeadCommand _sut;
        private readonly AddLeadModel _model;
        private readonly Guid _userId = Guid.NewGuid();
        private readonly Guid _listingId = Guid.NewGuid();
        private readonly ActiveListing _activeListing;
        private readonly AutoMocker _mocker;
        private readonly DateTimeOffset _createdDate = DateTimeOffset.UtcNow;

        public AddLeadCommand_should()
        {
            _mocker = new AutoMocker();
            _model = new AddLeadModel()
            {
                ListingId = _listingId
            };
            _activeListing = ListingMocks.ActiveListing_1;

            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindActive(_listingId))
                .Returns(Option<ActiveListing>.Some(_activeListing));

            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetCurrentUtcDateTime())
                .Returns(_createdDate);

            _sut = _mocker.CreateInstance<AddLeadCommand>();
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
        }
    }
}
