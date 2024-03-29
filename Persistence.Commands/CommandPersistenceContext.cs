﻿using Core.Domain.Listings;
using Core.Domain.Messages;
using Core.Domain.Profiles;
using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Persistence.Commands
{
    public class CommandPersistenceContext : DbContext
    {
        public DbSet<ActiveProfile> ActiveProfiles { get; set; }
        public DbSet<PassiveProfile> PassiveProfiles { get; set; }
        public DbSet<ActiveListing> ActiveListings { get; set; }
        public DbSet<ClosedListing> ClosedListings { get; set; }
        public DbSet<ImageReference> ImageReferences { get; set; }
        public DbSet<NewListing> NewListings { get; set; }
        public DbSet<PassiveListing> PassiveListings { get; set; }
        public DbSet<SuspiciousListing> SuspiciousListings { get; set; }
        public DbSet<Message> Messages { get; set; }

        public CommandPersistenceContext(DbContextOptions<CommandPersistenceContext> options)
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
