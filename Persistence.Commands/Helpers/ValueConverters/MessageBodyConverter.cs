using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Commands.Helpers.ValueConverters
{
    public sealed class MessageBodyConverter : ValueConverter<MessageBody, string>
    {
        public MessageBodyConverter()
            : base(
                domain => domain.Content,
                db => MessageBody.Create(db).ToUnsafeRight())
        {
        }
    }
}
