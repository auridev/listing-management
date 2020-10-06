using BusinessLine.Core.Domain.Common;
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
        public void be_NotSeen_if_value_is_null()
        {
            var seenDate = SeenDate.Create(null);
            seenDate.Should().BeOfType(typeof(NotSeen));
        }

        [Fact]
        public void be_NotSeen_if_value_is_min_DateTime_available()
        {
            var minDateTime = new DateTimeOffset(DateTime.MinValue, TimeSpan.Zero);

            var seenDate = SeenDate.Create(minDateTime);

            seenDate.Should().BeOfType(typeof(NotSeen));
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

        [Fact]
        public void have_NotSeen_with_default_Value_value()
        {
            var notSeen = SeenDate.Create(null);

            notSeen.Value.Should().Be(new DateTimeOffset(DateTime.MinValue, TimeSpan.Zero));
        }

        [Fact]
        public void have_CreateNone_for_explicit_NotSeen_creation()
        {
            var seenDate = SeenDate.CreateNone();

            seenDate.Should().BeOfType(typeof(NotSeen));
        }
    }
}
