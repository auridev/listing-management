using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Commands.Helpers.ValueConverters
{
    public sealed class CityConverter : ValueConverter<City, string>
    {
        public CityConverter()
            : base(
                domain => domain.Name.Value,
                db => City.Create(db).ToUnsafeRight())
        {
        }
    }
}
