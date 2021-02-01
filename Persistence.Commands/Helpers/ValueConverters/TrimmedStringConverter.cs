using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Commands.Helpers.ValueConverters
{
    public sealed class TrimmedStringConverter : ValueConverter<TrimmedString, string>
    {
        public TrimmedStringConverter()
            : base(
                  domain => domain.Value,
                  db => TrimmedString.Create(db).ToUnsafeRight())
        {
        }
    }
}
