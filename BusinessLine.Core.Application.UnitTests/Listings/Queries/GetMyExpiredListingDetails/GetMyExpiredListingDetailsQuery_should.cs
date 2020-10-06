using BusinessLine.Core.Application.Listings.Queries;
using BusinessLine.Core.Application.Listings.Queries.GetMyExpiredListingDetails;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Queries.GetMyExpiredListingDetails
{
    public class GetMyExpiredListingDetailsQuery_should
    {
        private readonly GetMyExpiredListingDetailsQuery _sut;
        private readonly GetMyExpiredListingDetailsQueryParams _queryParams;
        private readonly MyExpiredListingDetailsModel _model;
        private readonly AutoMocker _mocker;
        private readonly Guid _userId = Guid.NewGuid();
        private readonly Guid _listingId = Guid.NewGuid();

        public GetMyExpiredListingDetailsQuery_should()
        {
            _mocker = new AutoMocker();
            _queryParams = new GetMyExpiredListingDetailsQueryParams()
            {
                ListingId = _listingId
            };
            _model = new MyExpiredListingDetailsModel()
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
                ExpiredOn = DateTimeOffset.UtcNow.AddDays(10)
            };

            _mocker
                .GetMock<IListingDataService>()
                .Setup(s => s.FindMyExpired(_userId, _queryParams))
                .Returns(Option<MyExpiredListingDetailsModel>.Some(_model));


            _sut = _mocker.CreateInstance<GetMyExpiredListingDetailsQuery>();
        }

        [Fact]
        public void retrieve_my_expired_listing_details_from_dataservice()
        {
            Option<MyExpiredListingDetailsModel> model = _sut.Execute(_userId, _queryParams);

            _mocker
                .GetMock<IListingDataService>()
                .Verify(s => s.FindMyExpired(_userId, _queryParams), Times.Once);
        }

        [Fact]
        public void return_none_if_listing_details_does_not_exist()
        {
            // arrange
            _mocker
                .GetMock<IListingDataService>()
                .Setup(s => s.FindMyExpired(_userId, _queryParams))
                .Returns(Option<MyExpiredListingDetailsModel>.None);

            // act
            Option<MyExpiredListingDetailsModel> model = _sut.Execute(_userId, _queryParams);

            // assert
            model.IsNone.Should().BeTrue();
        }

        [Fact]
        public void return_none_if_query_params_is_not_valid()
        {
            // act
            Option<MyExpiredListingDetailsModel> model = _sut.Execute(_userId, null);

            // assert
            model.IsNone.Should().BeTrue();
        }
    }
}
