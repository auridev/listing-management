using BusinessLine.Core.Application.Listings.Queries;
using BusinessLine.Core.Application.Listings.Queries.GetMyPassiveListingDetails;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Queries.GetMyPassiveListingDetails
{
    public class GetMyPassiveListingDetailsQuery_should
    {
        private readonly GetMyPassiveListingDetailsQuery _sut;
        private readonly GetMyPassiveListingDetailsQueryParams _queryParams;
        private readonly MyPassiveListingDetailsModel _model;
        private readonly AutoMocker _mocker;
        private readonly Guid _userId = Guid.NewGuid();
        private readonly Guid _listingId = Guid.NewGuid();

        public GetMyPassiveListingDetailsQuery_should()
        {
            _mocker = new AutoMocker();
            _queryParams = new GetMyPassiveListingDetailsQueryParams()
            {
                ListingId = _listingId
            };
            _model = new MyPassiveListingDetailsModel()
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
                DeactivationDate = DateTimeOffset.UtcNow.AddDays(10),
                DeactivationReason = "asdasd"
            };

            _mocker
                .GetMock<IListingDataService>()
                .Setup(s => s.FindMyPassive(_userId, _queryParams))
                .Returns(Option<MyPassiveListingDetailsModel>.Some(_model));


            _sut = _mocker.CreateInstance<GetMyPassiveListingDetailsQuery>();
        }

        [Fact]
        public void retrieve_my_new_listing_details_from_dataservice()
        {
            Option<MyPassiveListingDetailsModel> model = _sut.Execute(_userId, _queryParams);

            _mocker
                .GetMock<IListingDataService>()
                .Verify(s => s.FindMyPassive(_userId, _queryParams), Times.Once);
        }

        [Fact]
        public void return_none_if_listing_details_does_not_exist()
        {
            _mocker
                .GetMock<IListingDataService>()
                .Setup(s => s.FindMyPassive(_userId, _queryParams))
                .Returns(Option<MyPassiveListingDetailsModel>.None);

            Option<MyPassiveListingDetailsModel> model = _sut.Execute(_userId, _queryParams);

            model.IsNone.Should().BeTrue();
        }

        [Fact]
        public void return_none_if_query_params_is_not_valid()
        {
            Option<MyPassiveListingDetailsModel> model = _sut.Execute(_userId, null);

            model.IsNone.Should().BeTrue();
        }
    }
}
