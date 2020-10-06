using BusinessLine.Core.Application.Listings.Queries;
using BusinessLine.Core.Application.Listings.Queries.GetPublicListings;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Queries.GetPublicListings
{
    public class GetPublicListingsQuery_should
    {
        private readonly GetPublicListingsQuery _sut;
        private readonly GetPublicListingsQueryParams _queryParams;
        private readonly ICollection<PublicListingModel> _model;
        private readonly AutoMocker _mocker;
        private readonly Guid _userId = Guid.NewGuid();

        public GetPublicListingsQuery_should()
        {
            _mocker = new AutoMocker();
            _model = new List<PublicListingModel>()
            {
                new PublicListingModel()
                {
                    Id = Guid.NewGuid(),
                    Title = "title",
                    MaterialType = "aaaa",
                    Country = "ania",
                    City = "ius",
                    Weight  = 23.7F
                }
    
            };
            _queryParams = new GetPublicListingsQueryParams()
            {
                PageNumber = 1,
                PageSize = 11,
                MaterialTypeIds = new int [] { 10, 20 },
                SearchParam = "asd"
            };
            _mocker
                .GetMock<IListingDataService>()
                .Setup(s => s.GetPublic(_userId, _queryParams))
                .Returns(_model);

            _sut = _mocker.CreateInstance<GetPublicListingsQuery>();
        }

        [Fact]
        public void return_model_collection()
        {
            ICollection<PublicListingModel> result = _sut.Execute(_userId, _queryParams);

            result.Should().NotBeNull();
        }

        [Fact]
        public void retrieve_collection_from_data_access_service()
        {
            ICollection<PublicListingModel> result = _sut.Execute(_userId, _queryParams);

            _mocker
                .GetMock<IListingDataService>()
                .Verify(s => s.GetPublic(_userId, _queryParams), Times.Once);
        }

        [Fact]
        public void return_empty_collection_if_queryParams_is_not_valid()
        {
            ICollection<PublicListingModel> result = _sut.Execute(_userId, null);

            result.Should().NotBeNull();
            result.Count.Should().Be(0);
        }
    }
}
