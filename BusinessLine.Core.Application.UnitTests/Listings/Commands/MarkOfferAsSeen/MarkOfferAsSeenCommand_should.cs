using BusinessLine.Common.Dates;
using BusinessLine.Core.Application.Listings.Commands;
using BusinessLine.Core.Application.Listings.Commands.MarkOfferAsSeen;
using BusinessLine.Core.Application.UnitTests.TestMocks;
using BusinessLine.Core.Domain.Listings;
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
        private readonly Offer _offer;
        private readonly AutoMocker _mocker;
        private readonly Guid _offerId = Guid.NewGuid();
        private readonly DateTimeOffset _seenDate = DateTimeOffset.UtcNow.AddDays(-1);

        public MarkOfferAsSeenCommand_should()
        {
            _mocker = new AutoMocker();
            _offer = ListingMocks.Offer_1;
            _model = new MarkOfferAsSeenModel()
            {
                OfferId = _offerId
            };

            _mocker
                .GetMock<IOfferRepository>()
                .Setup(r => r.Find(_offerId))
                .Returns(Option<Offer>.Some(_offer));

            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetCurrentUtcDateTime())
                .Returns(_seenDate);

            _sut = _mocker.CreateInstance<MarkOfferAsSeenCommand>();
        }

        [Fact]
        public void retrieve_offer_from_repository()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IOfferRepository>()
                .Verify(r => r.Find(_offerId), Times.Once);
        }

        [Fact]
        public void save_repository_changes()
        {
            _sut.Execute(_model);

            _mocker
                .GetMock<IOfferRepository>()
                .Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void do_nothing_if_offer_is_not_found()
        {
            _mocker
                .GetMock<IOfferRepository>()
                .Setup(r => r.Find(It.IsAny<Guid>()))
                .Returns(Option<Offer>.None);
 
            _sut.Execute(_model);

            _mocker
                .GetMock<IOfferRepository>()
                .Verify(r => r.Save(), Times.Never);
        }
    }
}
