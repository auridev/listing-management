using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Commands.Helpers.ValueConverters
{
    public sealed class PhoneConverter : ValueConverter<Phone, string>
    {
        public PhoneConverter()
            : base(
                domain => domain.Number,
                db => Phone.Create(db).ToUnsafeRight())
        {
        }
    }
}
