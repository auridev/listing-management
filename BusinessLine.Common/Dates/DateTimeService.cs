using System;

namespace BusinessLine.Common.Dates
{
    public class DateTimeService : IDateTimeService
    {
        public DateTimeOffset GetCurrentUtcDateTime()
        {
            return DateTimeOffset.UtcNow;
        }

        public DateTimeOffset GetFutureUtcDateTime(int daysInFuture)
        {
            if (daysInFuture <= 0)
                throw new ArgumentException(nameof(daysInFuture));

            return DateTimeOffset.UtcNow.AddDays(daysInFuture);
        }

        public DateTimeOffset GetPastUtcDateTime(int daysAgo)
        {
            if (daysAgo <= 0)
                throw new ArgumentException(nameof(daysAgo));

            daysAgo = -1 * daysAgo;
            return DateTimeOffset.UtcNow.AddDays(daysAgo);
        }
    }
}
