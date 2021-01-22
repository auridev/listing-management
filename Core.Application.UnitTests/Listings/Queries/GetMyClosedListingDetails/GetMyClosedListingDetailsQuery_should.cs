using Core.Application.Listings.Queries;
using Core.Application.Listings.Queries.Common;
using Core.Application.Listings.Queries.GetMyClosedListingDetails;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Queries.GetMyClosedListingDetails
{
    public class GetMyClosedListingDetailsQuery_should
    {
        private readonly GetMyClosedListingDetailsQuery _sut;
        private readonly MyClosedListingDetailsModel _model;
        private readonly AutoMocker _mocker;
        private readonly Guid _userId = Guid.NewGuid();
        private readonly Guid _listingId = Guid.NewGuid();

        public GetMyClosedListingDetailsQuery_should()
        {
            _mocker = new AutoMocker();
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
                CreatedDate = DateTimeOffset.UtcNow.AddDays(-10),
                ClosedOn = DateTimeOffset.UtcNow.AddDays(10),
                AcceptedOfferValue = 9M,
                AcceptedOfferCurrencyCode = "SSS",
                OfferOwnerContactDetails = new OfferOwnerContactDetailsModel()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Smith",
                    Phone = "121313",
                    Email = "email@email.com"
                }
            };

            _mocker
                .GetMock<IListingReadOnlyRepository>()
                .Setup(s => s.FindMyClosed(_userId, _listingId))
                .Returns(Option<MyClosedListingDetailsModel>.Some(_model));


            _sut = _mocker.CreateInstance<GetMyClosedListingDetailsQuery>();
        }

        [Fact]
        public void retrieve_my_closed_listing_details_from_dataservice()
        {
            Option<MyClosedListingDetailsModel> model = _sut.Execute(_userId, _listingId);

            _mocker
                .GetMock<IListingReadOnlyRepository>()
                .Verify(s => s.FindMyClosed(_userId, _listingId), Times.Once);
        }

        [Fact]
        public void return_none_if_listing_details_does_not_exist()
        {
            // arrange
            _mocker
                .GetMock<IListingReadOnlyRepository>()
                .Setup(s => s.FindMyClosed(_userId, _listingId))
                .Returns(Option<MyClosedListingDetailsModel>.None);

            // act
            Option<MyClosedListingDetailsModel> model = _sut.Execute(_userId, _listingId);

            // assert
            model.IsNone.Should().BeTrue();
        }

        public static IEnumerable<object[]> InvalidArguments => new List<object[]>
        {
            new object[] { Guid.NewGuid(), default },
            new object[] { default, Guid.NewGuid() }
        };

        [Theory]
        [MemberData(nameof(InvalidArguments))]
        public void reject_none_if_arguments_are_not_valid(Guid userId, Guid listingId)
        {
            // act
            Option<MyClosedListingDetailsModel> model = _sut.Execute(userId, listingId);

            // assert
            model.IsNone.Should().BeTrue();
        }
    }
}
