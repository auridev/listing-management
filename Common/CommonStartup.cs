using Common.Dates;
using Microsoft.Extensions.DependencyInjection;

namespace Common
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
