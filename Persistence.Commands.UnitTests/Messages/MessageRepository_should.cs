using Core.Domain.Common;
using Core.Domain.Messages;
using FluentAssertions;
using LanguageExt;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace Persistence.Commands.Messages.UnitTests
{
    public class MessageRepository_should
    {
        private readonly DbContextOptions<CommandPersistenceContext> _options;
        private readonly Message _message = new Message(
            new Guid("70bcb72a-166d-4960-9a8a-124b529e22cf"),
            Recipient.Create(Guid.NewGuid()),
            Subject.Create("my subject"),
            MessageBody.Create("message content"),
            Option<SeenDate>.None,
            DateTimeOffset.UtcNow);

        public MessageRepository_should()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            _options = new DbContextOptionsBuilder<CommandPersistenceContext>()
                .UseSqlite(connection)
                .Options;
        }

        [Fact]
        public void throw_exception_during_add_when_message_is_null()
        {
            using (var context = new CommandPersistenceContext(_options))
            {
                // Arrange
                var repository = new MessageRepository(context);

                // Act
                Action action = () => repository.Add(null);

                // Assert
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Fact]
        public void return_Some_if_message_exists()
        {
            using (var context = new CommandPersistenceContext(_options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.Messages.Add(_message);
                context.SaveChanges();
            }

            using (var context = new CommandPersistenceContext(_options))
            {
                // Arrange
                var repository = new MessageRepository(context);

                // Act
                Option<Message> optionalMessage = repository.Find(new Guid("70bcb72a-166d-4960-9a8a-124b529e22cf"));

                // Assert
                optionalMessage.IsSome.Should().BeTrue();
            }
        }

        [Fact]
        public void return_None_if_message_does_not_exist()
        {
            using (var context = new CommandPersistenceContext(_options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.Messages.Add(_message);
                context.SaveChanges();
            }

            using (var context = new CommandPersistenceContext(_options))
            {
                // Arrange
                var repository = new MessageRepository(context);

                // Act
                Option<Message> optionalMessage = repository.Find(Guid.NewGuid());

                // Assert
                optionalMessage.IsNone.Should().BeTrue();
            }
        }
    }
}
