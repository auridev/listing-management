using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class Email_should
    {
        [Fact]
        public void not_have_any_leading_or_trailing_whitespaces_in_Name_property()
        {
            Email
                .Create("  qqq@aaa.lt   ")
                .Right(email => email.Value.Should().Be("qqq@aaa.lt"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void return_EiherLeft_with_proper_error_during_creation_if_argument_is_not_valid(string value)
        {
            Either<Error, Email> eitherEmail = Email.Create(value);

            eitherEmail.IsLeft.Should().BeTrue();
            eitherEmail
               .Right(_ => throw InvalidExecutionPath.Exception)
               .Left(error => error.Should().BeOfType<Error.Invalid>());
        }

        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { null },
            new object[] { string.Empty },
            new object[] { "" },
            new object[] { default },
            new object[] { "aaaaa" },
            new object[] { "f@" },
            new object[] { "@" },
            new object[] { "@gg.com" },
            new object[] { "@." },
            new object[] { "a@." },
            new object[] { "a@com" }
        };

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_Values_match()
        {
            var first = Email.Create("aaa@google.com");
            var second = Email.Create("aaa@google.com");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_Values_match()
        {
            var first = (object)Email.Create("aaa@google.com");
            var second = (object)Email.Create("aaa@google.com");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }


        [Fact]
        public void be_treated_as_equal_using_equals_operator_if_Values_match()
        {
            var first = Email.Create("we@microsoft.org");
            var second = Email.Create("we@microsoft.org");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Values_dont_match()
        {
            var first = Email.Create("some@eta.net");
            var second = Email.Create("none@eta.net");

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
