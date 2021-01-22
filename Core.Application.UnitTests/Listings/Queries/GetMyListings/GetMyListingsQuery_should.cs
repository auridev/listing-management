using Core.Application.Helpers;
using Core.Application.Listings.Queries;
using Core.Application.Listings.Queries.GetMyListings;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Queries.GetMyListings
{
    public class GetMyListingsQuery_should
    {
        private readonly GetMyListingsQuery _sut;
        private readonly GetMyListingsQueryParams _queryParams;
        private readonly PagedList<MyListingModel> _model;
        private readonly AutoMocker _mocker;
        private readonly Guid _userId = Guid.NewGuid();

        public GetMyListingsQuery_should()
        {
            _mocker = new AutoMocker();
            _model = new PagedList<MyListingModel>(new List<MyListingModel>(), 1, 1, 1);

            _queryParams = new GetMyListingsQueryParams()
            {
                PageNumber = 1,
                PageSize = 11,
            };
            _mocker
                .GetMock<IListingReadOnlyRepository>()
                .Setup(s => s.GetMy(_userId, _queryParams))
                .Returns(_model);

            _sut = _mocker.CreateInstance<GetMyListingsQuery>();
        }


        [Fact]
        public void return_model_collection()
        {
            PagedList<MyListingModel> result = _sut.Execute(_userId, _queryParams);

            result.Should().NotBeNull();
        }

        [Fact]
        public void retrieve_collection_from_data_access_service()
        {
            PagedList<MyListingModel> result = _sut.Execute(_userId, _queryParams);

            _mocker
                .GetMock<IListingReadOnlyRepository>()
                .Verify(s => s.GetMy(_userId, _queryParams), Times.Once);
        }

        public static IEnumerable<object[]> InvalidArguments => new List<object[]>
        {
            new object[] { Guid.NewGuid(), null },
            new object[] { default, new GetMyListingsQueryParams() { PageNumber = 1, PageSize = 11 } }
        };

        [Theory]
        [MemberData(nameof(InvalidArguments))]
        public void reject_empty_list_if_arguments_are_not_valid(Guid userId, GetMyListingsQueryParams queryParams)
        {
            // act
            PagedList<MyListingModel> result = _sut.Execute(userId, queryParams);

            // assert
            result.Should().NotBeNull();
            result.Count.Should().Be(0);
        }
    }
}
