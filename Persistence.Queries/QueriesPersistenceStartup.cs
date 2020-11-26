using Core.Application.Messages.Queries;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Queries.Messages;

namespace Persistence.Queries
{
    public static class QueriesPersistenceStartup
    {
        public static IServiceCollection AddPersistenceForQueries(this IServiceCollection services)
        {
            services.AddScoped<IMessageQueryRepository, MessageQueryRepository>();

            return services;
        }
    }
}
