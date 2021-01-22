using Core.Application.Helpers;
using Core.Application.Profiles.Queries;
using Core.Application.Profiles.Queries.GetProfileList;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Profiles.Queries.GetProfileList
{
    public class GetProfileListQuery_should
    {
        private readonly GetProfileListQuery _sut;
        private readonly AutoMocker _mocker;
        private readonly GetProfileListQueryParams _queryParams;
        private readonly PagedList<ProfileModel> _model;

        public GetProfileListQuery_should()
        {
            _mocker = new AutoMocker();
            _queryParams = new GetProfileListQueryParams()
            {
                Search = "aaa",
                IsActive = null
            };
            _model = PagedList<ProfileModel>.CreateEmpty();

            _mocker
              .GetMock<IProfileReadOnlyRepository>()
              .Setup(s => s.Get(It.IsAny<GetProfileListQueryParams>()))
              .Returns(_model);


            _sut = _mocker.CreateInstance<GetProfileListQuery>();
        }

        [Fact]
        public void return_ProfileModel_collection()
        {
            PagedList<ProfileModel> result = _sut.Execute(_queryParams);

            result.Should().NotBeNull();
        }

        [Fact]
        public void retrieve_paged_list_from_repository()
        {
            PagedList<ProfileModel> result = _sut.Execute(_queryParams);

            _mocker
                .GetMock<IProfileReadOnlyRepository>()
                .Verify(s => s.Get(_queryParams), Times.Once);
        }

        [Fact]
        public void return_an_empty_paged_list_if_queryParams_is_not_valid()
        {
            PagedList<ProfileModel> result = _sut.Execute(null);

            result.Should().NotBeNull();
            result.Count.Should().Be(0);
        }
    }
}
