using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Commands.Helpers.ValueConverters
{
    public sealed class MaterialTypeConverter : ValueConverter<MaterialType, int>
    {
        public MaterialTypeConverter()
            : base(
                  domain => domain.Id,
                  db => MaterialType.ById(db).ToUnsafeRight())
        {
        }
    }
}
