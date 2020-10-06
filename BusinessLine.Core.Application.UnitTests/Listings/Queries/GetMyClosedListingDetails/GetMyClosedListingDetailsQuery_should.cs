using BusinessLine.Core.Application.Listings.Queries;
using BusinessLine.Core.Application.Listings.Queries.Common;
using BusinessLine.Core.Application.Listings.Queries.GetMyClosedListingDetails;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Queries.GetMyClosedListingDetails
{
    public class GetMyClosedListingDetailsQuery_should
    {
        private readonly GetMyClosedListingDetailsQuery _sut;
        private readonly GetMyClosedListingDetailsQueryParams _queryParams;
        private readonly MyClosedListingDetailsModel _model;
        private readonly AutoMocker _mocker;
        private readonly Guid _userId = Guid.NewGuid();
        private readonly Guid _listingId = Guid.NewGuid();

        public GetMyClosedListingDetailsQuery_should()
        {
            _mocker = new AutoMocker();
            _queryParams = new GetMyClosedListingDetailsQueryParams()
            {
                ListingId = _listingId
            };
            _model = new MyClosedListingDetailsModel()
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
                ClosedOn = DateTimeOffset.UtcNow.AddDays(10),
                AcceptedOffer = new OfferDetailsModel()
                {
                    Id = Guid.NewGuid(),
                    Value = 9M,
                    CurrencyCode = "SSS"
                },
                OfferOwnerContactDetails = new OfferOwnerContactDetailsModel()
                {
                    FirstName = "John",
                    LastName = "Smith",
                    Phone = "121313",
                    Email = "email@email.com"
                }
            };

            _mocker
                .GetMock<IListingDataService>()
                .Setup(s => s.FindMyClosed(_userId, _queryParams))
                .Returns(Option<MyClosedListingDetailsModel>.Some(_model));


            _sut = _mocker.CreateInstance<GetMyClosedListingDetailsQuery>();
        }

        [Fact]
        public void retrieve_my_closed_listing_details_from_dataservice()
        {
            Option<MyClosedListingDetailsModel> model = _sut.Execute(_userId, _queryParams);

            _mocker
                .GetMock<IListingDataService>()
                .Verify(s => s.FindMyClosed(_userId, _queryParams), Times.Once);
        }

        [Fact]
        public void return_none_if_listing_details_does_not_exist()
        {
            // arrange
            _mocker
                .GetMock<IListingDataService>()
                .Setup(s => s.FindMyClosed(_userId, _queryParams))
                .Returns(Option<MyClosedListingDetailsModel>.None);

            // act
            Option<MyClosedListingDetailsModel> model = _sut.Execute(_userId, _queryParams);

            // assert
            model.IsNone.Should().BeTrue();
        }

        [Fact]
        public void return_none_if_query_params_is_not_valid()
        {
            // act
            Option<MyClosedListingDetailsModel> model = _sut.Execute(_userId, null);

            // assert
            model.IsNone.Should().BeTrue();
        }
    }
}
