using Core.Application.Profiles.Commands;
using Core.Domain.Profiles;
using LanguageExt;
using System;

namespace Persistence.Commands.Profiles
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly PersistenceContext _context;

        public ProfileRepository(PersistenceContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public void Add(ActiveProfile activeProfile)
        {
            _context.ActiveProfiles.Add(activeProfile);
        }

        public void Add(PassiveProfile passiveProfile)
        {
            throw new NotImplementedException();
        }

        public void Delete(ActiveProfile activeProfile)
        {
            throw new NotImplementedException();
        }

        public Option<ActiveProfile> Find(Guid id)
        {
            var activeProfile = _context.ActiveProfiles.Find(id);

            return activeProfile != null
                ? Option<ActiveProfile>.Some(activeProfile)
                : Option<ActiveProfile>.None;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(ActiveProfile activeProfile)
        {
            throw new NotImplementedException();
        }
    }
}
