using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Commands.Helpers.ValueConverters
{
    public sealed class CurrencyCodeConverter : ValueConverter<CurrencyCode, string>
    {
        public CurrencyCodeConverter()
            : base(
                domain => domain.Value,
                db => CurrencyCode.Create(db).ToUnsafeRight())
        {
        }
    }
}
