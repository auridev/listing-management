using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Commands.Helpers.ValueConverters
{
    public sealed class FileSizeConverter : ValueConverter<FileSize, long>
    {
        public FileSizeConverter()
           : base(
               domain => domain.Bytes,
               db => FileSize.Create(db).ToUnsafeRight())
        {
        }
    }
}
