using Common.Dates;
using Common.FileSystem;
using Microsoft.Extensions.DependencyInjection;

namespace Common
{
    public static class CommonStartup
    {
        public static IServiceCollection AddCommon(this IServiceCollection services)
        {
            services.AddScoped<IDateTimeService, DateTimeService>();
            services.AddScoped<IFileSystemService, FileSystemService>();

            return services;
        }
    }
}
