using BusinessLine.Core.Domain;
using BusinessLine.Core.Domain.Profiles;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BusinessLine.Persistence
{
    public class PersistenceContext : DbContext
    {
        public DbSet<ActiveProfile> ActiveProfiles { get; set; }

        public DbSet<PassiveProfile> PassiveProfiles { get; set; }

        public PersistenceContext(DbContextOptions<PersistenceContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
