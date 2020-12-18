using Core.Application.Helpers;
using Core.Application.Listings.Queries;
using Core.Application.Listings.Queries.GetPublicListings;
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
        private readonly PagedList<PublicListingModel> _model;
        private readonly AutoMocker _mocker;
        private readonly Guid _userId = Guid.NewGuid();

        public GetPublicListingsQuery_should()
        {
            _mocker = new AutoMocker();
            _model = new PagedList<PublicListingModel>(new List<PublicListingModel>(), 1, 1, 1);
            
            _queryParams = new GetPublicListingsQueryParams()
            {
                PageNumber = 1,
                PageSize = 11,
                MaterialTypeIds = new int [] { 10, 20 },
                SearchParam = "asd",
                OnlyWithMyOffers = false
            };
            _mocker
                .GetMock<IListingReadOnlyRepository>()
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
                .GetMock<IListingReadOnlyRepository>()
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
