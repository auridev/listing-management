using Common.ApplicationSettings;
using Common.Dates;
using Common.FileSystem;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common
{
    public static class CommonStartup
    {
        public static IServiceCollection AddCommon(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDateTimeService, DateTimeService>();
            services.AddScoped<IFileSystemService, FileSystemService>();

            services.Configure<ImageRepositorySettings>(configuration.GetSection("ImageRepository"));

            return services;
        }
    }
}
