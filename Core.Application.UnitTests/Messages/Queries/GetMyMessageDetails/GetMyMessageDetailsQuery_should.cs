using Core.Application.Messages.Queries;
using Core.Application.Messages.Queries.GetMyMessageDetails;
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
        private readonly MyMessageDetailsModel _model;
        private readonly AutoMocker _mocker;
        private readonly Guid _userId = Guid.NewGuid();
        private readonly Guid _messageId = Guid.NewGuid();

        public GetMyMessageDetailsQuery_should()
        {
            _mocker = new AutoMocker();
            _model = new MyMessageDetailsModel()
            {
                Id = Guid.NewGuid(),
                Subject = "subjec",
                Body = "body",
                CreatedDate = DateTimeOffset.UtcNow,
                Seen = true
            };

            _mocker
                .GetMock<IMessageReadOnlyRepository>()
                .Setup(s => s.Find(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Option<MyMessageDetailsModel>.Some(_model));

            _sut = _mocker.CreateInstance<GetMyMessageDetailsQuery>();
        }

        [Fact]
        public void retrieve_message_from_repository()
        {
            Option<MyMessageDetailsModel> model = _sut.Execute(_userId, _messageId);

            _mocker
                .GetMock<IMessageReadOnlyRepository>()
                .Verify(s => s.Find(_userId, _messageId), Times.Once);
        }

        [Fact]
        public void return_none_if_message_does_not_exist()
        {
            // arrange
            _mocker
                .GetMock<IMessageReadOnlyRepository>()
                .Setup(s => s.Find(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Option<MyMessageDetailsModel>.None);

            // act
            Option<MyMessageDetailsModel> model = _sut.Execute(_userId, _messageId);

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
        public void reject_none_if_arguments_are_not_valid(Guid userId, Guid _messageId)
        {
            // act
            Option<MyMessageDetailsModel> model = _sut.Execute(userId, _messageId);

            // assert
            model.IsNone.Should().BeTrue();
        }
    }
}
