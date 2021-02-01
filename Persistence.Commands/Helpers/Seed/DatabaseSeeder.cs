using LanguageExt;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Persistence.Commands.Helpers.Seed
{
    public static class DatabaseSeeder
    {
        public static void EnsureDatabaseSeeded(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var context = serviceScope.ServiceProvider.GetService<CommandPersistenceContext>())
            {
                context.Database.EnsureCreated();

                Seed(context);
            }
        }

        private static void Seed(CommandPersistenceContext context)
        {
            AddMessages(context);

            AddActiveProfiles(context);

            AddPassiveProfiles(context);

            AddNewListings(context);

            AddActiveListings(context);

            AddPassiveListings(context);


            context.SaveChanges();
        }

        private static void AddMessages(CommandPersistenceContext context)
        {
            if (context.Messages.Any())
                return;

            context.Messages.AddRange(SeedData.Messages.ForPeter);
            context.Messages.AddRange(SeedData.Messages.ForJohn);
        }

        private static void AddActiveProfiles(CommandPersistenceContext context)
        {
            if (context.ActiveProfiles.Any())
                return;

            context.ActiveProfiles.AddRange(
                SeedData.Personas.Peter.ActiveProfile,
                SeedData.Personas.John.ActiveProfile,
                SeedData.Personas.Alice.ActiveProfile);
        }
        private static void AddPassiveProfiles(CommandPersistenceContext context)
        {
            if (context.PassiveProfiles.Any())
                return;

            context.PassiveProfiles.AddRange(
                SeedData.Personas.Mark.PassiveProfile,
                SeedData.Personas.Philip.PassiveProfile);
        }

        private static void AddNewListings(CommandPersistenceContext context)
        {
            if (context.NewListings.Any())
                return;

            context.NewListings.AddRange(SeedData.NewListings);
        }

        private static void AddActiveListings(CommandPersistenceContext context)
        {
            if (context.ActiveListings.Any())
                return;

            context.ActiveListings.AddRange(SeedData.ActiveListings);
        }

        private static void AddPassiveListings(CommandPersistenceContext context)
        {
            if (context.PassiveListings.Any())
                return;

            context.PassiveListings.AddRange(SeedData.PassiveListings);
        }
    }
}
