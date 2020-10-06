using BusinessLine.Common.Dates;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLine.Common
{
    public static class CommonStartup
    {
        public static IServiceCollection AddCommon(this IServiceCollection services)
        {
            services.AddScoped<IDateTimeService, DateTimeService>();

            return services;
        }
    }
}
