using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Commands.Helpers.ValueConverters
{
    public sealed class Alpha2CodeConverter : ValueConverter<Alpha2Code, string>
    {
        public Alpha2CodeConverter()
            : base(
                domain => domain.Value,
                db => Alpha2Code.Create(db).ToUnsafeRight())
        {
        }
    }
}
