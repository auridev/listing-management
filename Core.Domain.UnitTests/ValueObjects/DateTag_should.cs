using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using System;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class DateTag_should
    {
        private readonly DateTimeOffset _dateTime = new DateTimeOffset(2020, 1, 3, 0, 0, 0, 0, TimeSpan.Zero);
       
        [Fact]
        public void have_Year_property()
        {
            DateTag
                .Create(_dateTime)
                .Right(tag => tag.Year.Should().Be(2020))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_Month_property()
        {
            DateTag
               .Create(_dateTime)
               .Right(tag => tag.Month.Should().Be(1))
               .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_Day_property()
        {
            DateTag
               .Create(_dateTime)
               .Right(tag => tag.Day.Should().Be(3))
               .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_Date_values_match()
        {
            var first = DateTag.Create(new DateTimeOffset(2000, 5, 8, 2, 3 ,4, TimeSpan.Zero));
            var second = DateTag.Create(new DateTimeOffset(2000, 5, 8, 22, 33, 44, TimeSpan.Zero));

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_Date_values_match()
        {
            var first = (object)DateTag.Create(new DateTimeOffset(2000, 5, 8, 2, 3, 4, TimeSpan.Zero));
            var second = (object)DateTag.Create(new DateTimeOffset(2000, 5, 8, 22, 33, 44, TimeSpan.Zero));

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
