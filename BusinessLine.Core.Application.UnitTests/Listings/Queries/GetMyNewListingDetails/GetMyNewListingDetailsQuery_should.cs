using Core.Application.Listings.Queries;
using Core.Application.Listings.Queries.GetMyNewListingDetails;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Queries.GetMyNewListingDetails
{
    public class GetMyNewListingDetailsQuery_should
    {
        private readonly GetMyNewListingDetailsQuery _sut;
        private readonly GetMyNewListingDetailsQueryParams _queryParams;
        private readonly MyNewListingDetailsModel _model;
        private readonly AutoMocker _mocker;
        private readonly Guid _userId = Guid.NewGuid();
        private readonly Guid _listingId = Guid.NewGuid();

        public GetMyNewListingDetailsQuery_should()
        {
            _mocker = new AutoMocker();
            _queryParams = new GetMyNewListingDetailsQueryParams()
            {
                ListingId = _listingId
            };
            _model = new MyNewListingDetailsModel()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Title = "title",
                MaterialTypeId = 10,
                Weight = 2.3F,
                MassUnit = "kg",
                Description = "description",
                FirstName = "firstname",
                LastName = "lasname",
                Company = "cccc",
                Phone = "+333 111 22222",
                CountryCode = "dddd",
                State = "45",
                City = "obeliai",
                PostCode = "12",
                Address = "asd",
                Latitude = 1.1D,
                Longitude = 2.2D,
                CreatedOn = DateTimeOffset.UtcNow.AddDays(10)
            };

            _mocker
                .GetMock<IListingDataService>()
                .Setup(s => s.FindMyNew(_userId, _queryParams))
                .Returns(Option<MyNewListingDetailsModel>.Some(_model));


            _sut = _mocker.CreateInstance<GetMyNewListingDetailsQuery>();
        }

        [Fact]
        public void retrieve_my_new_listing_details_from_dataservice()
        {
            Option<MyNewListingDetailsModel> model = _sut.Execute(_userId, _queryParams);

            _mocker
                .GetMock<IListingDataService>()
                .Verify(s => s.FindMyNew(_userId, _queryParams), Times.Once);
        }

        [Fact]
        public void return_none_if_listing_details_does_not_exist()
        {
            _mocker
                .GetMock<IListingDataService>()
                .Setup(s => s.FindMyNew(_userId, _queryParams))
                .Returns(Option<MyNewListingDetailsModel>.None);

            Option<MyNewListingDetailsModel> model = _sut.Execute(_userId, _queryParams);

            model.IsNone.Should().BeTrue();
        }

        [Fact]
        public void return_none_if_query_params_is_not_valid()
        {
            Option<MyNewListingDetailsModel> model = _sut.Execute(_userId, null);

            model.IsNone.Should().BeTrue();
        }
    }
}
