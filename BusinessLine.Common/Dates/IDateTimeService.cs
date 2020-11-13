using System;

namespace Common.Dates
{
    public interface IDateTimeService
    {
        DateTimeOffset GetCurrentUtcDateTime();
        DateTimeOffset GetFutureUtcDateTime(int daysInFuture);
        DateTimeOffset GetPastUtcDateTime(int daysAgo);
    }
}