using BusinessLine.Core.Application.Profiles.Queries;
using BusinessLine.Core.Application.Profiles.Queries.GetProfileList;
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
        private readonly ICollection<ProfileModel> _model;
        private readonly Guid _userId = Guid.NewGuid();

        public GetProfileListQuery_should()
        {
            _mocker = new AutoMocker();
            _queryParams = new GetProfileListQueryParams()
            {
                Search = "aaa",
                IsActive = null
            };
            _model = new ProfileModel[]
            {
                new ProfileModel()
            };

            _mocker
              .GetMock<IProfileDataService>()
              .Setup(s => s.Get(_userId, It.IsAny<GetProfileListQueryParams>()))
              .Returns(_model);


            _sut = _mocker.CreateInstance<GetProfileListQuery>();
        }

        [Fact]
        public void return_ProfileModel_collection()
        {
            ICollection<ProfileModel> result = _sut.Execute(_userId, _queryParams);

            result.Should().NotBeNull();
        }

        [Fact]
        public void retrieve_the_collection_from_data_access_service()
        {
            ICollection<ProfileModel> result = _sut.Execute(_userId, _queryParams);

            _mocker
                .GetMock<IProfileDataService>()
                .Verify(s => s.Get(_userId, _queryParams), Times.Once);
        }

        [Fact]
        public void return_an_empty_collection_if_queryParams_is_not_valid()
        {
            ICollection<ProfileModel> result = _sut.Execute(_userId, null);

            result.Should().NotBeNull();
            result.Count.Should().Be(0);
        }
    }
}
