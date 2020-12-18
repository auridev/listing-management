using Core.Application.Listings.Queries;
using Core.Application.Listings.Queries.Common;
using Core.Application.Listings.Queries.GetPublicListingDetails;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Queries.GetPublicListingDetails
{
    public class GetPublicListingDetailsQuery_should
    {
        private readonly GetPublicListingDetailsQuery _sut;
        private readonly PublicListingDetailsModel _model;
        private readonly AutoMocker _mocker;
        private readonly Guid _userId = Guid.NewGuid();
        private readonly Guid _listingId = Guid.NewGuid();

        public GetPublicListingDetailsQuery_should()
        {
            _mocker = new AutoMocker();
            _model = new PublicListingDetailsModel()
            {
                Id = Guid.NewGuid(),
                Title = "title",
                MaterialTypeId = 10,
                Weight = 2.3F,
                MassUnit = "kg",
                Description = "description",
                City = "obeliai",
                MyOffer = new OfferDetailsModel()
            };

            _mocker
                .GetMock<IListingReadOnlyRepository>()
                .Setup(s => s.FindPublic(_userId, _listingId))
                .Returns(Option<PublicListingDetailsModel>.Some(_model));

            _sut = _mocker.CreateInstance<GetPublicListingDetailsQuery>();
        }

        [Fact]
        public void retrieve_my_new_listing_details_from_dataservice()
        {
            Option<PublicListingDetailsModel> model = _sut.Execute(_userId, _listingId);

            _mocker
                .GetMock<IListingReadOnlyRepository>()
                .Verify(s => s.FindPublic(_userId, _listingId), Times.Once);
        }

        [Fact]
        public void return_none_if_listing_details_does_not_exist()
        {
            _mocker
                .GetMock<IListingReadOnlyRepository>()
                .Setup(s => s.FindPublic(_userId, _listingId))
                .Returns(Option<PublicListingDetailsModel>.None);

            Option<PublicListingDetailsModel> model = _sut.Execute(_userId, _listingId);

            model.IsNone.Should().BeTrue();
        }
    }
}
