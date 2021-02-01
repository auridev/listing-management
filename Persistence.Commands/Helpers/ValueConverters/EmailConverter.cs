using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Commands.Helpers.ValueConverters
{
    public sealed class EmailConverter : ValueConverter<Email, string>
    {
        public EmailConverter()
            : base(
                domain => domain.Value,
                db => Email.Create(db).ToUnsafeRight())
        {
        }
    }
}
