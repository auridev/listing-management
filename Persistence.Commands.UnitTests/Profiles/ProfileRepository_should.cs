using Core.Domain.Common;
using Core.Domain.Profiles;
using Core.UnitTests.Mocks;
using FluentAssertions;
using LanguageExt;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace Persistence.Commands.Profiles.UnitTests
{
    public class ProfileRepository_should
    {
        private readonly DbContextOptions<CommandPersistenceContext> _options;
        private readonly ActiveProfile _fakeActiveProfile = new ActiveProfile(
            new Guid("4413c264-b7f5-4b0b-a50c-23523c7a61d1"),
            Guid.NewGuid(),
            FakesCollection.Email,
            FakesCollection.ContactDetails,
            FakesCollection.LocationDetails,
            FakesCollection.GeographicLocation,
            FakesCollection.UserPreferences,
            DateTimeOffset.UtcNow,
            Option<SeenDate>.None);

        public ProfileRepository_should()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            _options = new DbContextOptionsBuilder<CommandPersistenceContext>()
                .UseSqlite(connection)
                .Options;
        }

        [Fact]
        public void throw_exception_when_adding_a_null_active_profile()
        {
            using (var context = new CommandPersistenceContext(_options))
            {
                // Arrange
                var repository = new ProfileRepository(context);

                // Act
                ActiveProfile activeProfile = null;
                Action action = () => repository.Add(activeProfile);

                // Assert
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void throw_exception_when_adding_a_null_passive_profile()
        {
            using (var context = new CommandPersistenceContext(_options))
            {
                // Arrange
                var repository = new ProfileRepository(context);

                // Act
                PassiveProfile passiveProfile = null;
                Action action = () => repository.Add(passiveProfile);

                // Assert
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void throw_exception_when_deleting_a_null_active_profile()
        {
            using (var context = new CommandPersistenceContext(_options))
            {
                // Arrange
                var repository = new ProfileRepository(context);

                // Act
                Action action = () => repository.Delete(null);

                // Assert
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void return_Some_if_active_profile_exists()
        {
            using (var context = new CommandPersistenceContext(_options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.ActiveProfiles.Add(_fakeActiveProfile);
                context.SaveChanges();
            }

            using (var context = new CommandPersistenceContext(_options))
            {
                // Arrange
                var repository = new ProfileRepository(context);

                // Act
                Option<ActiveProfile> optionalActiveProfile = repository.Find(new Guid("4413c264-b7f5-4b0b-a50c-23523c7a61d1"));

                // Assert
                optionalActiveProfile.IsSome.Should().BeTrue();
            }
        }

        [Fact]
        public void return_None_if_active_profile_does_not_exist()
        {
            using (var context = new CommandPersistenceContext(_options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
            }

            using (var context = new CommandPersistenceContext(_options))
            {
                // Arrange
                var repository = new ProfileRepository(context);

                // Act
                Option<ActiveProfile> optionalActiveProfile = repository.Find(Guid.NewGuid());

                // Assert
                optionalActiveProfile.IsNone.Should().BeTrue();
            }
        }
    }
}
