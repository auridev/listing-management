using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class MessageBody_should
    {
        [Fact]
        public void have_Content_property()
        {
            MessageBody
                .Create("I want to dance with somebody")
                .Right(body => body.Content.Should().Be("I want to dance with somebody"))
                .Left(_ => throw InvalidExecutionPath.Exception);
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
        public void return_EiherLeft_with_proper_error_during_creation_if_argument_is_not_valid(string content)
        {
            Either<Error, MessageBody> eitherMessageBody = MessageBody.Create(content);

            eitherMessageBody.IsLeft.Should().BeTrue();
            eitherMessageBody
               .Right(_ => throw InvalidExecutionPath.Exception)
               .Left(error => error.Should().BeOfType<Error.Invalid>());
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
