using BusinessLine.Core.Domain.Common;
using FluentAssertions;
using System;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class DateTag_should
    {
        private readonly DateTimeOffset _dateTime;

        public DateTag_should()
        {
            _dateTime = new DateTimeOffset(2020, 1, 3, 0, 0, 0, 0, TimeSpan.Zero);
        }

        [Fact]
        public void have_Year_property()
        {
            var dateTimeTag = DateTag.Create(_dateTime);
            dateTimeTag.Year.Should().Be(2020);
        }

        [Fact]
        public void have_Month_property()
        {
            var dateTimeTag = DateTag.Create(_dateTime);
            dateTimeTag.Month.Should().Be(1);
        }

        [Fact]
        public void have_Day_property()
        {
            var dateTimeTag = DateTag.Create(_dateTime);
            dateTimeTag.Day.Should().Be(3);
        }

        [Fact]
        public void be_treated_as_equal_using_Equals_method_if_Date_values_match()
        {
            var first = DateTag.Create(new DateTimeOffset(2000, 5, 8, 2, 3 ,4, TimeSpan.Zero));
            var second = DateTag.Create(new DateTimeOffset(2000, 5, 8, 22, 33, 44, TimeSpan.Zero));

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Date_values_match()
        {
            var first = DateTag.Create(new DateTimeOffset(2000, 5, 8, 2, 3, 4, TimeSpan.Zero));
            var second = DateTag.Create(new DateTimeOffset(2000, 5, 8, 22, 33, 44, TimeSpan.Zero));

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Date_values_dont_match()
        {
            var first = DateTag.Create(new DateTimeOffset(2001, 5, 8, 0, 0, 0, TimeSpan.Zero));
            var second = DateTag.Create(new DateTimeOffset(2000, 5, 8, 0, 0, 0, TimeSpan.Zero));

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
