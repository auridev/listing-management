using BusinessLine.Core.Application;
using BusinessLine.Core.Application.Profiles.Commands;
using BusinessLine.Persistence.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLine.Persistence
{
    public static class PersistenceStartup
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<PersistenceContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(PersistenceContext).Assembly.FullName)));

            services.AddScoped<IProfileRepository, ProfileRepository>();

            return services;
        }
    }
}
