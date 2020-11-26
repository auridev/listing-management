using Core.Application.Messages.Commands;
using Core.Domain.Messages;
using LanguageExt;
using System;

namespace Persistence.Commands.Messages
{
    // all the Find methods use explicit casting to either Some or None
    // implicit cast wokrs as well but I want this to be more readable 
    public class MessageRepository : IMessageRepository
    {
        private readonly CommandPersistenceContext _context;

        public MessageRepository(CommandPersistenceContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public void Add(Message message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            _context.Messages.Add(message);
        }

        public Option<Message> Find(Guid id)
        {
            Message message = _context.Messages.Find(id);

            return message != null
                ? Option<Message>.Some(message)
                : Option<Message>.None;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Message message)
        {

        }
    }
}
