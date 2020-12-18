using Core.Application.Helpers;
using Core.Application.Messages.Queries;
using Core.Application.Messages.Queries.GetMyMessages;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Messages.Queries.GetMyMessages
{
    public class GetMyMessagesQuery_should
    {
        private readonly GetMyMessagesQuery _sut;
        private readonly GetMyMessagesQueryParams _queryParams;
        private readonly PagedList<MyMessageModel> _model;
        private readonly AutoMocker _mocker;
        private readonly Guid _userId = Guid.NewGuid();

        public GetMyMessagesQuery_should()
        {
            _mocker = new AutoMocker();
            _model = new PagedList<MyMessageModel>(new List<MyMessageModel>(), 1, 1, 1);
            _queryParams = new GetMyMessagesQueryParams()
            {
                PageNumber = 1,
                PageSize = 11,
            };
            _mocker
                .GetMock<IMessageReadOnlyRepository>()
                .Setup(s => s.Get(_userId, _queryParams))
                .Returns(_model);

            _sut = _mocker.CreateInstance<GetMyMessagesQuery>();
        }


        [Fact]
        public void return_paged_model_list()
        {
            PagedList<MyMessageModel> result = _sut.Execute(_userId, _queryParams);

            result.Should().NotBeNull();
        }

        [Fact]
        public void retrieve_paged_list_from_repository()
        {
            PagedList<MyMessageModel> result = _sut.Execute(_userId, _queryParams);

            _mocker
                .GetMock<IMessageReadOnlyRepository>()
                .Verify(s => s.Get(_userId, _queryParams), Times.Once);
        }

        [Fact]
        public void return_empty_paged_list_if_queryParams_is_not_valid()
        {
            PagedList<MyMessageModel> result = _sut.Execute(_userId, null);

            result.Should().NotBeNull();
            result.Count.Should().Be(0);
        }
    }
}
