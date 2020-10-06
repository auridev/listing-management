using BusinessLine.Core.Application.Profiles.Commands.CreateProfile;
using BusinessLine.Core.Application.Profiles.Commands.CreateProfile.Factory;
using BusinessLine.Core.Application.Profiles.Commands.MarkProfileAsIntroduced;
using BusinessLine.Core.Application.Profiles.Queries.GetProfileDetails;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLine.Core.Application
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
