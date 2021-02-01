using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Commands.Helpers.ValueConverters
{
    public sealed class PostCodeConverter : ValueConverter<PostCode, string>
    {
        public PostCodeConverter()
            : base(
                domain => domain.Value.Value,
                db => PostCode.Create(db).ToUnsafeRight())
        {
        }
    }
}
