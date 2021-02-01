using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Commands.Helpers.ValueConverters
{
    public class FileNameConverter : ValueConverter<FileName, string>
    {
        public FileNameConverter()
           : base(
               domain => domain.Value,
               db => FileName.Create(db).ToUnsafeRight())
        {
        }
    }
}
