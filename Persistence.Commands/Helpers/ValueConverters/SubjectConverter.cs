using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Commands.Helpers.ValueConverters
{
    public sealed class SubjectConverter : ValueConverter<Subject, string>
    {
        public SubjectConverter()
            : base(
                domain => domain.Value.Value,
                db => Subject.Create(db).ToUnsafeRight())
        {
        }
    }
}
