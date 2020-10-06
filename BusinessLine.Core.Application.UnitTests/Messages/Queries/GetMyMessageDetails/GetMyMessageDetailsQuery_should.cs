using BusinessLine.Core.Application.Messages.Queries;
using BusinessLine.Core.Application.Messages.Queries.GetMyMessageDetails;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Messages.Queries.GetMyMessageDetails
{
    public class GetMyMessageDetailsQuery_should
    {
        private readonly GetMyMessageDetailsQuery _sut;
        private readonly GetMyMessageDetailsQueryParams _queryParams;
        private readonly MyMessageDetailsModel _model;
        private readonly AutoMocker _mocker;
        private readonly Guid _userId = Guid.NewGuid();
        private readonly Guid _messageId = Guid.NewGuid();

        public GetMyMessageDetailsQuery_should()
        {
            _mocker = new AutoMocker();
            _queryParams = new GetMyMessageDetailsQueryParams()
            {
                MessageId = _messageId
            };
            _model = new MyMessageDetailsModel()
            {
                Id = Guid.NewGuid(),
                Subject = "subjec",
                Body = "body",
                Params = new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>("id","123"),
                    new KeyValuePair<string, string>("name","adadadssad"),
                },
                Seen = true
            };

            _mocker
                .GetMock<IMessageDataService>()
                .Setup(s => s.Find(_userId, _queryParams))
                .Returns(Option<MyMessageDetailsModel>.Some(_model));

            _sut = _mocker.CreateInstance<GetMyMessageDetailsQuery>();
        }

        [Fact]
        public void retrieve_message_from_dataservice()
        {
            Option<MyMessageDetailsModel> model = _sut.Execute(_userId, _queryParams);

            _mocker
                .GetMock<IMessageDataService>()
                .Verify(s => s.Find(_userId, _queryParams), Times.Once);
        }

        [Fact]
        public void return_none_if_message_does_not_exist()
        {
            // arrange
            _mocker
                .GetMock<IMessageDataService>()
                .Setup(s => s.Find(_userId, _queryParams))
                .Returns(Option<MyMessageDetailsModel>.None);

            // act
            Option<MyMessageDetailsModel> model = _sut.Execute(_userId, _queryParams);

            // assert
            model.IsNone.Should().BeTrue();
        }

        [Fact]
        public void return_none_if_query_params_is_not_valid()
        {
            // act
            Option<MyMessageDetailsModel> model = _sut.Execute(_userId, null);

            // assert
            model.IsNone.Should().BeTrue();
        }
    }
}
