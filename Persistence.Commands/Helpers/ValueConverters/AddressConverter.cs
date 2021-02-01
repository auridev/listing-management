using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Commands.Helpers.ValueConverters
{
    public sealed class AddressConverter : ValueConverter<Address, string>
    {
        public AddressConverter()
            : base(
                domain => domain.Value.Value,
                db => Address.Create(db).ToUnsafeRight())
        {
        }
    }
}
