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
        private readonly MyNewListingDetailsModel _model;
        private readonly AutoMocker _mocker;
        private readonly Guid _userId = Guid.NewGuid();
        private readonly Guid _listingId = Guid.NewGuid();

        public GetMyNewListingDetailsQuery_should()
        {
            _mocker = new AutoMocker();
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
                CreatedDate = DateTimeOffset.UtcNow.AddDays(10)
            };

            _mocker
                .GetMock<IListingReadOnlyRepository>()
                .Setup(s => s.FindMyNew(_userId, _listingId))
                .Returns(Option<MyNewListingDetailsModel>.Some(_model));


            _sut = _mocker.CreateInstance<GetMyNewListingDetailsQuery>();
        }

        [Fact]
        public void retrieve_my_new_listing_details_from_dataservice()
        {
            Option<MyNewListingDetailsModel> model = _sut.Execute(_userId, _listingId);

            _mocker
                .GetMock<IListingReadOnlyRepository>()
                .Verify(s => s.FindMyNew(_userId, _listingId), Times.Once);
        }

        [Fact]
        public void return_none_if_listing_details_does_not_exist()
        {
            _mocker
                .GetMock<IListingReadOnlyRepository>()
                .Setup(s => s.FindMyNew(_userId, _listingId))
                .Returns(Option<MyNewListingDetailsModel>.None);

            Option<MyNewListingDetailsModel> model = _sut.Execute(_userId, _listingId);

            model.IsNone.Should().BeTrue();
        }
    }
}
