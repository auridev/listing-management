using Core.Domain.Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class FileSize_should
    {
        [Fact]
        public void have_Bytes_property()
        {
            var fileSize = FileSize.Create(20L);
            fileSize.Bytes.Should().Be(20L);
        }

        [Theory]
        [MemberData(nameof(InvalidArgument))]
        public void throw_an_exception_if_argument_is_not_valid(long size)
        {
            Action create = () => FileSize.Create(size);

            create.Should().Throw<ArgumentException>();
        }

        public static IEnumerable<object[]> InvalidArgument => new List<object[]>
        {
            new object[] { -1 },
            new object[] { 0 },
            new object[] { default }
        };

        [Fact]
        public void be_treated_as_equal_using_generic_equals_method_if_values_match()
        {
            var first = FileSize.Create(15L);
            var second = FileSize.Create(15L);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_equals_method_if_values_match()
        {
            var first = (object) FileSize.Create(1L);
            var second = (object) FileSize.Create(1L);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_values_match()
        {
            var first = FileSize.Create(15L);
            var second = FileSize.Create(15L);

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_values_dont_match()
        {
            var first = FileSize.Create(8L);
            var second = FileSize.Create(9L);

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
