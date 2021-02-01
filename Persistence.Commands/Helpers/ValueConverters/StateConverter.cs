using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Commands.Helpers.ValueConverters
{
    public sealed class StateConverter : ValueConverter<State, string>
    {
        public StateConverter()
            : base(
                domain => domain.Name.Value,
                db => State.Create(db).ToUnsafeRight())
        {
        }
    }
}
