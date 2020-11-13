using Core.Domain.Profiles;
using LanguageExt;
using System;

namespace Core.Application.Profiles.Commands
{
    public interface IProfileRepository
    {
        Option<ActiveProfile> Find(Guid id);
        void Add(ActiveProfile activeProfile);
        void Add(PassiveProfile passiveProfile);
        void Delete(ActiveProfile activeProfile);
        void Update(ActiveProfile activeProfile);
        void Save();
    }
}
