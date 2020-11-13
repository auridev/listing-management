using Common.Dates;
using FluentAssertions;
using System;
using Xunit;

namespace BusinessLines.Common.UnitTests.Dates
{
    public class DateTimeService_should
    {
        private readonly DateTimeService _sut;

        public DateTimeService_should()
        {
            _sut = new DateTimeService();
        }

        [Fact]
        public void return_current_utc_datetime()
        {

            DateTimeOffset currentUtc = _sut.GetCurrentUtcDateTime();

            currentUtc.Should().BeCloseTo(DateTimeOffset.UtcNow);
        }

        [Fact]
        public void return_future_utc_datetime()
        {
            DateTimeOffset futureUtc = _sut.GetFutureUtcDateTime(3);

            futureUtc.Should().BeCloseTo(DateTimeOffset.UtcNow.AddDays(3));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void thrown_an_exception_on_GetFutureUtcDateTime_if_days_argument_is_zero_or_less(int daysInFuture)
        {
            Action action = () => _sut.GetFutureUtcDateTime(daysInFuture);

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void return_past_utc_datetime()
        {
            DateTimeOffset pastUtc = _sut.GetPastUtcDateTime(20);

            pastUtc.Should().BeCloseTo(DateTimeOffset.UtcNow.AddDays(-20));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void thrown_an_exception_on_GetPastUtcDateTime_if_days_argument_is_zero_or_less(int daysAgo)
        {
            Action action = () => _sut.GetPastUtcDateTime(daysAgo);

            action.Should().Throw<ArgumentException>();
        }
    }
}
