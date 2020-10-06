using BusinessLine.Common.Dates;
using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Profiles;
using LanguageExt;
using System;

namespace BusinessLine.Core.Application.Profiles.Commands.MarkProfileAsIntroduced
{
    public sealed class MarkProfileAsIntroducedCommand : IMarkProfileAsIntroducedCommand
    {
        private readonly IProfileRepository _repository;
        private readonly IDateTimeService _dateTimeService;

        public MarkProfileAsIntroducedCommand(IProfileRepository repository, IDateTimeService dateTimeService)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public void Execute(MarkProfileAsIntroducedModel model)
        {
            // Command
            DateTimeOffset dateTimeOffset = _dateTimeService.GetCurrentUtcDateTime();
            SeenDate seenDate = SeenDate.Create(dateTimeOffset);
            Option<ActiveProfile> optionalProfile = _repository.Find(model.ProfileId);

            optionalProfile
                .Some(profile =>
                {
                    profile.HasSeenIntroduction(seenDate);

                    _repository.Update(profile);

                    _repository.Save();
                })
                .None(() => { });
        }
    }
}
