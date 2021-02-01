using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Commands.Helpers.ValueConverters
{
    public sealed class DistanceMeasurementUnitConverter : ValueConverter<DistanceMeasurementUnit, string>
    {
        public DistanceMeasurementUnitConverter()
           : base(
               domain => domain.Symbol,
               db => DistanceMeasurementUnit.BySymbol(db).ToUnsafeRight())
        {
        }
    }
}
