using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Commands.Helpers.ValueConverters
{
    public sealed class TitleConverter : ValueConverter<Title, string>
    {
        public TitleConverter()
            : base(
                  domain => domain.Value.Value,
                  db => Title.Create(db).ToUnsafeRight())
        {
        }
    }
}
