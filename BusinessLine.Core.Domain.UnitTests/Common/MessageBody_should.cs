using Core.Domain.Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class MessageBody_should
    {
        private readonly MessageBody _sut;


        public MessageBody_should()
        {
            _sut = MessageBody.Create("I want to dance with somebody");
        }

        [Fact]
        public void have_Content_property()
        {
            _sut.Content.Should().Be("I want to dance with somebody");
        }

        [Fact]
        public void be_treated_as_equal_using_generic_equals_method_if_contents_match()
        {
            var first = MessageBody.Create("aaaa");
            var second = MessageBody.Create("aaaa");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_equals_method_if_contents_match()
        {
            var first = (object) MessageBody.Create("bb");
            var second = (object) MessageBody.Create("bb");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_contents_match()
        {
            var first = MessageBody.Create("cccccc");
            var second = MessageBody.Create("cccccc");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_contents_dont_match()
        {
            var first = MessageBody.Create("1");
            var second = MessageBody.Create("2");

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(InvalidArguments))]
        public void throw_an_exception_during_creation_if_value_is_not_valid(string content)
        {
            Action createAction = () => MessageBody.Create(content);

            createAction.Should().Throw<ArgumentException>();
        }

        public static IEnumerable<object[]> InvalidArguments => new List<object[]>
        {
            new object[] { null },
            new object[] { "" },
            new object[] { default },
            new object[] { string.Empty },
            new object[] { "   " }
        };
    }
}
