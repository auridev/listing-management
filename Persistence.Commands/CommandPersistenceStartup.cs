using Core.Application.Listings.Commands;
using Core.Application.Profiles.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Commands.Listings;
using Persistence.Commands.Profiles;

namespace Persistence.Commands
{
    public static class CommandPersistenceStartup
    {
        public static IServiceCollection AddPersistenceForCommands(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CommandPersistenceContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("BusinessLine"),
                    b => b.MigrationsAssembly(typeof(CommandPersistenceContext).Assembly.FullName)));

            services.AddScoped<IProfileRepository, ProfileRepository>();

            services.AddScoped<IListingRepository, ListingRepository>();

            services.AddScoped<IImagePersistenceService, ImagePersistenceService>();

            return services;
        }
    }
}
