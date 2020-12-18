using Core.Application.Listings.Queries;
using Core.Application.Messages.Queries;
using Core.Application.Profiles.Queries;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Queries.Listings;
using Persistence.Queries.Listings.Factory;
using Persistence.Queries.Messages;
using Persistence.Queries.Profiles;
using Persistence.Queries.Profiles.Factory;

namespace Persistence.Queries
{
    public static class QueriesPersistenceStartup
    {
        public static IServiceCollection AddPersistenceForQueries(this IServiceCollection services)
        {
            services
                .AddScoped<IProfileQueryFactory, ProfileQueryFactory>();
            services
                .AddScoped<IMessageReadOnlyRepository, MessageReadOnlyRepository>();
            services
                .AddScoped<IProfileReadOnlyRepository, ProfileReadOnlyRepository>();
            services
                .AddScoped<IListingReadOnlyRepository, ListingReadOnlyRepository>();
            services
                .AddScoped<IListingQueryFactory, ListingQueryFactory>();

            return services;
        }
    }
}
