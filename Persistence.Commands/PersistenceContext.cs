using Core.Domain.Common;
using Core.Domain.Listings;
using Core.Domain.Messages;
using Core.Domain.Profiles;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Persistence.Commands
{
    public class PersistenceContext : DbContext
    {
        public DbSet<ActiveProfile> ActiveProfiles { get; set; }
        public DbSet<PassiveProfile> PassiveProfiles { get; set; }
        public DbSet<ActiveListing> ActiveListings { get; set; }
        public DbSet<ClosedListing> ClosedListings { get; set; }
        public DbSet<ListingImageReference> ListingImageReferences { get; set; }
        public DbSet<NewListing> NewListings { get; set; }
        public DbSet<PassiveListing> PassiveListings { get; set; }
        public DbSet<SuspiciousListing> SuspiciousListings { get; set; }
        public DbSet<Message> Messages { get; set; }

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
