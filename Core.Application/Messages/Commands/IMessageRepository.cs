using Core.Domain.Messages;
using LanguageExt;
using System;

namespace Core.Application.Messages.Commands
{
    public interface IMessageRepository
    {
        Option<Message> Find(Guid id);
        void Add(Message message);
        void Save();
        void Update(Message message);
    }
}
