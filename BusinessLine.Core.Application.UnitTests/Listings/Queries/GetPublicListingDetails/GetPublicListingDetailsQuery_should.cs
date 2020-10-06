using BusinessLine.Core.Application.Listings.Queries;
using BusinessLine.Core.Application.Listings.Queries.GetPublicListingDetails;
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
        private readonly GetPublicListingDetailsQueryParams _queryParams;
        private readonly PublicListingDetailsModel _model;
        private readonly AutoMocker _mocker;
        private readonly Guid _userId = Guid.NewGuid();
        private readonly Guid _listingId = Guid.NewGuid();

        public GetPublicListingDetailsQuery_should()
        {
            _mocker = new AutoMocker();
            _queryParams = new GetPublicListingDetailsQueryParams()
            {
                ListingId = _listingId
            };
            _model = new PublicListingDetailsModel()
            {
                Id = Guid.NewGuid(),
                Title = "title",
                MaterialTypeId = 10,
                Weight = 2.3F,
                MassUnit = "kg",
                Description = "description",
                City = "obeliai"
            };

            _mocker
                .GetMock<IListingDataService>()
                .Setup(s => s.FindPublic(_userId, _queryParams))
                .Returns(Option<PublicListingDetailsModel>.Some(_model));

            _sut = _mocker.CreateInstance<GetPublicListingDetailsQuery>();
        }

        [Fact]
        public void retrieve_my_new_listing_details_from_dataservice()
        {
            Option<PublicListingDetailsModel> model = _sut.Execute(_userId, _queryParams);

            _mocker
                .GetMock<IListingDataService>()
                .Verify(s => s.FindPublic(_userId, _queryParams), Times.Once);
        }

        [Fact]
        public void return_none_if_listing_details_does_not_exist()
        {
            _mocker
                .GetMock<IListingDataService>()
                .Setup(s => s.FindPublic(_userId, _queryParams))
                .Returns(Option<PublicListingDetailsModel>.None);

            Option<PublicListingDetailsModel> model = _sut.Execute(_userId, _queryParams);

            model.IsNone.Should().BeTrue();
        }

        [Fact]
        public void return_none_if_query_params_is_not_valid()
        {
            Option<PublicListingDetailsModel> model = _sut.Execute(_userId, null);

            model.IsNone.Should().BeTrue();
        }
    }
}
