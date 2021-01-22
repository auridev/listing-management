using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class FileName_should
    {
        [Fact]
        public void have_all_Value_characters_in_lowercase()
        {
            FileName
                .Create("BBB")
                .Right(fileName => fileName.Value.ToString().Should().Be("bbb"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Theory]
        [MemberData(nameof(InvalidArguments))]
        public void return_EiherLeft_with_proper_error_during_creation_if_argument_is_not_valid(string extension)
        {
            Either<Error, FileName> eitherFileName = FileName.Create(extension);

            eitherFileName.IsLeft.Should().BeTrue();
            eitherFileName
               .Right(_ => throw InvalidExecutionPath.Exception)
               .Left(error => error.Should().BeOfType<Error.Invalid>());
        }

        public static IEnumerable<object[]> InvalidArguments => new List<object[]>
        {
            new object[] { null },
            new object[] { string.Empty },
            new object[] { "" },
            new object[] { default }
        };

        [Fact]
        public void be_treated_as_equal_using_generic_equals_method_if_values_match()
        {
            var first = FileName.Create("bbb");
            var second = FileName.Create("bbb");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_equals_method_if_values_match()
        {
            var first = (object)FileName.Create("bbb");
            var second = (object)FileName.Create("bbb");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Values_match()
        {
            var first = FileName.Create("txt");
            var second = FileName.Create("txt");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Values_dont_match()
        {
            var first = FileName.Create("txt1");
            var second = FileName.Create("txt2");

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
