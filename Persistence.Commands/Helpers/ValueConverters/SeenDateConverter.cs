using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace Persistence.Commands.Helpers.ValueConverters
{
    public sealed class SeenDateConverter : ValueConverter<SeenDate, DateTimeOffset>
    {
        public SeenDateConverter()
            : base(
                domain => domain.Value,
                db => SeenDate.Create(db).ToUnsafeRight())
        {
        }
    }
}
