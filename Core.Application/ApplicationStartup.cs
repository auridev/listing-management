using Core.Application.Profiles.Commands.CreateProfile;
using Core.Application.Profiles.Commands.CreateProfile.Factory;
using Core.Application.Profiles.Commands.MarkProfileAsIntroduced;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application
{
    public static class ApplicationStartup
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICreateProfileCommand, CreateProfileCommand>();
            services.AddScoped<IMarkProfileAsIntroducedCommand, MarkProfileAsIntroducedCommand>();

            //services.AddScoped<IGetProfileDetailsQuery, GetProfileDetailsQuery>();

            services.AddScoped<IProfileFactory, ProfileFactory>();

            return services;
        }
    }
}
