using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Commands.Helpers.ValueConverters
{
    public sealed class MassMeasurementUnitConverter : ValueConverter<MassMeasurementUnit, string>
    {
        public MassMeasurementUnitConverter()
            : base(
                  domain => domain.Symbol,
                  db => MassMeasurementUnit.BySymbol(db).ToUnsafeRight())
        {
        }
    }
}
