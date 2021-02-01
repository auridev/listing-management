using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace Persistence.Commands.Helpers.ValueConverters
{
    public sealed class OwnerConverter : ValueConverter<Owner, Guid>
    {
        public OwnerConverter()
            : base(
                  domain => domain.UserId,
                  db => Owner.Create(db).ToUnsafeRight())
        {
        }
    }
}
