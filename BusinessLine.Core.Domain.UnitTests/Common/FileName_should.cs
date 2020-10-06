using BusinessLine.Core.Domain.Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class FileName_should
    {
        [Fact]
        public void have_a_Value_property()
        {
            var fileName = FileName.Create("aaa");
            fileName.Value.ToString().Should().Be("aaa");
        }

        [Fact]
        public void have_all_Value_characters_in_lowercase()
        {
            var fileName = FileName.Create( "BBB");
            fileName.Value.ToString().Should().Be("bbb");
        }


        [Theory]
        [MemberData(nameof(InvalidArguments))]
        public void throw_an_exception_if_arguments_are_not_valid(string extension)
        {
            Action action = () => FileName.Create(extension);

            action.Should().Throw<ArgumentNullException>();
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
