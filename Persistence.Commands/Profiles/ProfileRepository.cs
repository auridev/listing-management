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
            if (activeProfile == null)
                throw new ArgumentNullException(nameof(activeProfile));

            _context.ActiveProfiles.Add(activeProfile);
        }

        public void Add(PassiveProfile passiveProfile)
        {
            if (passiveProfile == null)
                throw new ArgumentNullException(nameof(passiveProfile));

            _context.PassiveProfiles.Add(passiveProfile);
        }

        public void Delete(ActiveProfile activeProfile)
        {
            if (activeProfile == null)
                throw new ArgumentNullException(nameof(activeProfile));

            _context.ActiveProfiles.Remove(activeProfile);
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

        // there's no need for implementation because of how EF tracks changes
        public void Update(ActiveProfile activeProfile)
        {
            
        }
    }
}
