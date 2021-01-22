using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace Persistence.Commands.Listings.UnitTests
{
    public class ListingRepository_should
    {
        private readonly DbContextOptions<CommandPersistenceContext> _options;

        public ListingRepository_should()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            _options = new DbContextOptionsBuilder<CommandPersistenceContext>()
                .UseSqlite(connection)
                .Options;
        }

        [Fact]
        public void throw_exception_during_add_when_new_listing_is_null()
        {
            using (var context = new CommandPersistenceContext(_options))
            {
                // Arrange
                var repository = new ListingRepository(context);
                NewListing newListing = null;

                // Act
                Action action = () => repository.Add(newListing, new List<ImageReference> { });

                // Assert
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void throw_exception_during_add_when_listing_image_references_is_null()
        {
            using (var context = new CommandPersistenceContext(_options))
            {
                // Arrange
                var repository = new ListingRepository(context);

                // Act
                Action action = () => repository.Add(DummyData.NewListing_1, null);

                // Assert
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void throw_exception_during_add_when_active_listing_is_null()
        {
            using (var context = new CommandPersistenceContext(_options))
            {
                // Arrange
                var repository = new ListingRepository(context);
                ActiveListing activeListing = null;

                // Act
                Action action = () => repository.Add(activeListing);

                // Assert
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void throw_exception_during_add_when_passive_listing_is_null()
        {
            using (var context = new CommandPersistenceContext(_options))
            {
                // Arrange
                var repository = new ListingRepository(context);
                PassiveListing passiveListing = null;

                // Act
                Action action = () => repository.Add(passiveListing);

                // Assert
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void throw_exception_during_add_when_suspicious_listing_is_null()
        {
            using (var context = new CommandPersistenceContext(_options))
            {
                // Arrange
                var repository = new ListingRepository(context);
                SuspiciousListing suspiciousListing = null;

                // Act
                Action action = () => repository.Add(suspiciousListing);

                // Assert
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void throw_exception_during_add_when_closed_listing_is_null()
        {
            using (var context = new CommandPersistenceContext(_options))
            {
                // Arrange
                var repository = new ListingRepository(context);
                ClosedListing closedListing = null;

                // Act
                Action action = () => repository.Add(closedListing);

                // Assert
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void throw_exception_during_delete_when_new_listing_is_null()
        {
            using (var context = new CommandPersistenceContext(_options))
            {
                // Arrange
                var repository = new ListingRepository(context);
                NewListing newListing = null;

                // Act
                Action action = () => repository.Delete(newListing);

                // Assert
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void throw_exception_during_active_when_active_listing_is_null()
        {
            using (var context = new CommandPersistenceContext(_options))
            {
                // Arrange
                var repository = new ListingRepository(context);
                ActiveListing activeListing = null;

                // Act
                Action action = () => repository.Delete(activeListing);

                // Assert
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void throw_exception_during_active_when_passive_listing_is_null()
        {
            using (var context = new CommandPersistenceContext(_options))
            {
                // Arrange
                var repository = new ListingRepository(context);
                PassiveListing passiveListing = null;

                // Act
                Action action = () => repository.Delete(passiveListing);

                // Assert
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void throw_exception_during_active_when_suspicious_listing_is_null()
        {
            using (var context = new CommandPersistenceContext(_options))
            {
                // Arrange
                var repository = new ListingRepository(context);
                SuspiciousListing suspiciousListing = null;

                // Act
                Action action = () => repository.Delete(suspiciousListing);

                // Assert
                action.Should().Throw<ArgumentNullException>();
            }
        }

        public static IEnumerable<object[]> FindActiveListingArguments => new List<object[]>
        {
            new object[]
            {
                new ActiveListing(
                    new Guid("67fd3c72-a71d-4d46-8a62-635baade03e7"),
                    TestValueObjectFactory.CreateOwner(Guid.NewGuid()),
                    DummyData.ListingDetails,
                    DummyData.ContactDetails,
                    DummyData.LocationDetails,
                    DummyData.GeographicLocation,
                    DateTimeOffset.UtcNow,
                    DateTimeOffset.UtcNow),
                "67fd3c72-a71d-4d46-8a62-635baade03e7",
                true
            },
            new object[]
            {
                new ActiveListing(
                    new Guid("11111111-1111-1111-1111-111111111111"),
                    TestValueObjectFactory.CreateOwner(Guid.NewGuid()),
                    DummyData.ListingDetails,
                    DummyData.ContactDetails,
                    DummyData.LocationDetails,
                    DummyData.GeographicLocation,
                    DateTimeOffset.UtcNow,
                    DateTimeOffset.UtcNow),
                "22222222-2222-2222-2222-222222222222",
                false
            }
        };

        [Theory]
        [MemberData(nameof(FindActiveListingArguments))]
        public void return_correct_option_depending_on_whether_active_listing_exists_or_not(ActiveListing listingToAdd, string idToFind, bool shouldExist)
        {
            using (var context = new CommandPersistenceContext(_options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.ActiveListings.Add(listingToAdd);
                context.SaveChanges();
            }

            using (var context = new CommandPersistenceContext(_options))
            {
                var repository = new ListingRepository(context);

                Option<ActiveListing> optionalActiveListing =
                    repository.FindActive(Guid.Parse(idToFind));

                if (shouldExist)
                    optionalActiveListing.IsSome.Should().BeTrue();
                else
                    optionalActiveListing.IsNone.Should().BeTrue();
            }
        }

        public static IEnumerable<object[]> FindNewListingArguments => new List<object[]>
        {
            new object[]
            {
                new NewListing(
                    new Guid("75ad2698-204c-449e-b04f-a57cccf6c755"),
                    TestValueObjectFactory.CreateOwner(Guid.NewGuid()),
                    DummyData.ListingDetails,
                    DummyData.ContactDetails,
                    DummyData.LocationDetails,
                    DummyData.GeographicLocation,
                    DateTimeOffset.UtcNow),
                "75ad2698-204c-449e-b04f-a57cccf6c755",
                true
            },
            new object[]
            {
                new NewListing(
                    new Guid("11111111-1111-1111-1111-111111111111"),
                    TestValueObjectFactory.CreateOwner(Guid.NewGuid()),
                    DummyData.ListingDetails,
                    DummyData.ContactDetails,
                    DummyData.LocationDetails,
                    DummyData.GeographicLocation,
                    DateTimeOffset.UtcNow),
                "22222222-2222-2222-2222-222222222222",
                false
            }
        };

        [Theory]
        [MemberData(nameof(FindNewListingArguments))]
        public void return_correct_option_depending_on_whether_new_listing_exists_or_not(NewListing listingToAdd, string idToFind, bool shouldExist)
        {
            using (var context = new CommandPersistenceContext(_options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.NewListings.Add(listingToAdd);
                context.SaveChanges();
            }

            using (var context = new CommandPersistenceContext(_options))
            {
                var repository = new ListingRepository(context);

                Option<NewListing> optionalNewListing = repository.FindNew(Guid.Parse(idToFind));

                // Assert
                if (shouldExist)
                    optionalNewListing.IsSome.Should().BeTrue();
                else
                    optionalNewListing.IsNone.Should().BeTrue();
            }
        }

        public static IEnumerable<object[]> FindPassiveListingArguments => new List<object[]>
        {
            new object[]
            {
                new PassiveListing(
                    new Guid("d63be5b2-a70d-4cd9-b5e9-8cb390152a76"),
                    TestValueObjectFactory.CreateOwner(Guid.NewGuid()),
                    DummyData.ListingDetails,
                    DummyData.ContactDetails,
                    DummyData.LocationDetails,
                    DummyData.GeographicLocation,
                    DateTimeOffset.UtcNow,
                    DateTimeOffset.UtcNow,
                    TestValueObjectFactory.CreateTrimmedString("too passive")),
                "d63be5b2-a70d-4cd9-b5e9-8cb390152a76",
                true
            },
            new object[]
            {
                new PassiveListing(
                    new Guid("11111111-1111-1111-1111-111111111111"),
                    TestValueObjectFactory.CreateOwner(Guid.NewGuid()),
                    DummyData.ListingDetails,
                    DummyData.ContactDetails,
                    DummyData.LocationDetails,
                    DummyData.GeographicLocation,
                    DateTimeOffset.UtcNow,
                    DateTimeOffset.UtcNow,
                    TestValueObjectFactory.CreateTrimmedString("too passive")),
                "22222222-2222-2222-2222-222222222222",
                false
            }
        };

        [Theory]
        [MemberData(nameof(FindPassiveListingArguments))]
        public void return_correct_option_depending_on_whether_passive_listing_exists_or_not(PassiveListing listingToAdd, string idToFind, bool shouldExist)
        {
            using (var context = new CommandPersistenceContext(_options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.PassiveListings.Add(listingToAdd);
                context.SaveChanges();
            }

            using (var context = new CommandPersistenceContext(_options))
            {
                var repository = new ListingRepository(context);

                Option<PassiveListing> optionalPassiveListing = repository.FindPassive(Guid.Parse(idToFind));

                // Assert
                if (shouldExist)
                    optionalPassiveListing.IsSome.Should().BeTrue();
                else
                    optionalPassiveListing.IsNone.Should().BeTrue();
            }
        }

        public static IEnumerable<object[]> FindSuspiciousListingArguments => new List<object[]>
        {
            new object[]
            {
                new SuspiciousListing(
                    new Guid("25fbbb10-ddec-4d48-adbc-4c5f4aed84a4"),
                    TestValueObjectFactory.CreateOwner(Guid.NewGuid()),
                    DummyData.ListingDetails,
                    DummyData.ContactDetails,
                    DummyData.LocationDetails,
                    DummyData.GeographicLocation,
                    DateTimeOffset.UtcNow,
                    DateTimeOffset.UtcNow,
                    TestValueObjectFactory.CreateTrimmedString("qwerty")),
                "25fbbb10-ddec-4d48-adbc-4c5f4aed84a4",
                true
            },
            new object[]
            {
                new SuspiciousListing(
                    new Guid("11111111-1111-1111-1111-111111111111"),
                    TestValueObjectFactory.CreateOwner(Guid.NewGuid()),
                    DummyData.ListingDetails,
                    DummyData.ContactDetails,
                    DummyData.LocationDetails,
                    DummyData.GeographicLocation,
                    DateTimeOffset.UtcNow,
                    DateTimeOffset.UtcNow,
                    TestValueObjectFactory.CreateTrimmedString("qwerty")),
                "22222222-2222-2222-2222-222222222222",
                false
            }
        };

        [Theory]
        [MemberData(nameof(FindSuspiciousListingArguments))]
        public void return_correct_option_depending_on_whether_suspicious_listing_exists_or_not(SuspiciousListing listingToAdd, string idToFind, bool shouldExist)
        {
            using (var context = new CommandPersistenceContext(_options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.SuspiciousListings.Add(listingToAdd);
                context.SaveChanges();
            }

            using (var context = new CommandPersistenceContext(_options))
            {
                var repository = new ListingRepository(context);

                Option<SuspiciousListing> optionalSuspiciousListing = repository.FindSuspicious(Guid.Parse(idToFind));

                // Assert
                if (shouldExist)
                    optionalSuspiciousListing.IsSome.Should().BeTrue();
                else
                    optionalSuspiciousListing.IsNone.Should().BeTrue();
            }
        }
    }
}
