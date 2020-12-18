using Core.Application.Listings.Queries;
using Core.Application.Listings.Queries.Common;
using Core.Application.Listings.Queries.GetMyActiveListingDetails;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Queries.GetMyActiveListingDetails
{
    public class GetMyActiveListingDetailsQuery_should
    {
        private readonly GetMyActiveListingDetailsQuery _sut;
        private readonly MyActiveListingDetailsModel _model;
        private readonly AutoMocker _mocker;
        private readonly Guid _userId = Guid.NewGuid();
        private readonly Guid _listingId = Guid.NewGuid();

        public GetMyActiveListingDetailsQuery_should()
        {
            _mocker = new AutoMocker();
            _model = new MyActiveListingDetailsModel()
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
                CreatedDate = DateTimeOffset.UtcNow.AddDays(-10),
                ExpirationDate = DateTimeOffset.UtcNow.AddDays(10),
                ReceivedOffers = new List<OfferDetailsModel> 
                {
                    new OfferDetailsModel()
                    {
                        Id = Guid.NewGuid(),
                        Value = 1.2M,
                        CurrencyCode = "ASD"
                    },
                    new OfferDetailsModel()
                    {
                        Id = Guid.NewGuid(),
                        Value = 23.2M,
                        CurrencyCode = "DSA"
                    },
                }
            };

            _mocker
                .GetMock<IListingReadOnlyRepository>()
                .Setup(s => s.FindMyActive(_userId, _listingId))
                .Returns(Option<MyActiveListingDetailsModel>.Some(_model));


            _sut = _mocker.CreateInstance<GetMyActiveListingDetailsQuery>();
        }

        [Fact]
        public void retrieve_my_active_listing_details_from_dataservice()
        {
            Option<MyActiveListingDetailsModel> model = _sut.Execute(_userId, _listingId);

            _mocker
                .GetMock<IListingReadOnlyRepository>()
                .Verify(s => s.FindMyActive(_userId, _listingId), Times.Once);
        }

        [Fact]
        public void return_none_if_listing_details_does_not_exist()
        {
            // arrange
            _mocker
                .GetMock<IListingReadOnlyRepository>()
                .Setup(s => s.FindMyActive(_userId, _listingId))
                .Returns(Option<MyActiveListingDetailsModel>.None);

            // act
            Option<MyActiveListingDetailsModel> model = _sut.Execute(_userId, _listingId);

            // assert
            model.IsNone.Should().BeTrue();
        }
    }
}
