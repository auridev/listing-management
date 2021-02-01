using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Commands.Helpers.ValueConverters
{
    public sealed class DescriptionConverter : ValueConverter<Description, string>
    {
        public DescriptionConverter()
            : base(
                  domain => domain.Value.Value,
                  db => Description.Create(db).ToUnsafeRight())
        {
        }
    }
}
