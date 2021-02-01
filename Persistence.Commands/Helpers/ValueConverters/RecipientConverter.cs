using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace Persistence.Commands.Helpers.ValueConverters
{
    public sealed class RecipientConverter : ValueConverter<Recipient, Guid>
    {
        public RecipientConverter()
            : base(
                domain => domain.UserId,
                db => Recipient.Create(db).ToUnsafeRight())
        {
        }
    }
}
