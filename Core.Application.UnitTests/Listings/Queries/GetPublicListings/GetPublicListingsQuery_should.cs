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
                MaterialTypeIds = new int[] { 10, 20 },
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
            PagedList<PublicListingModel> result = _sut.Execute(_userId, _queryParams);

            result.Should().NotBeNull();
        }

        [Fact]
        public void retrieve_collection_from_data_access_service()
        {
            PagedList<PublicListingModel> result = _sut.Execute(_userId, _queryParams);

            _mocker
                .GetMock<IListingReadOnlyRepository>()
                .Verify(s => s.GetPublic(_userId, _queryParams), Times.Once);
        }

        public static IEnumerable<object[]> InvalidArguments => new List<object[]>
        {
            new object[] { Guid.NewGuid(), null },
            new object[] { default, new GetPublicListingsQueryParams() { PageNumber = 1, PageSize = 11 } }
        };

        [Theory]
        [MemberData(nameof(InvalidArguments))]
        public void reject_empty_list_if_arguments_are_not_valid(Guid userId, GetPublicListingsQueryParams queryParams)
        {
            // act
            PagedList<PublicListingModel> result = _sut.Execute(userId, queryParams);

            // assert
            result.Should().NotBeNull();
            result.Count.Should().Be(0);
        }
    }
}
