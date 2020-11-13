using Core.Domain.Common;
using FluentAssertions;
using System;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class SeenDate_should
    {
        [Fact]
        public void have_a_Value_property()
        {
            var seenDate = SeenDate.Create(DateTimeOffset.Now.AddDays(-1));
            seenDate.Value.Should().BeCloseTo(DateTimeOffset.Now.AddDays(-1));
        }

        [Fact]
        public void be_treated_as_equal_using_Equals_method_if_DateTimeOffset_values_match()
        {
            var dateTimeOffset = DateTimeOffset.UtcNow;
            var first = SeenDate.Create(dateTimeOffset);
            var second = SeenDate.Create(dateTimeOffset);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_DateTimeOffset_values_match()
        {
            var dateTimeOffset = DateTimeOffset.UtcNow;
            var first = SeenDate.Create(dateTimeOffset);
            var second = SeenDate.Create(dateTimeOffset);

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_DateTimeOffset_values_dont_match()
        {
            var first = SeenDate.Create(DateTimeOffset.Now.AddDays(-2));
            var second = SeenDate.Create(DateTimeOffset.Now.AddDays(-3));

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
