using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Commands.Helpers.ValueConverters
{
    public sealed class CompanyConverter : ValueConverter<Company, string>
    {
        public CompanyConverter()
            : base(
                domain => domain.Name.Value,
                db => Company.Create(db).ToUnsafeRight())
        {
        }
    }
}
