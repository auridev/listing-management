﻿using BusinessLine.Common.Dates;
using BusinessLine.Core.Domain.Common;
using System;

namespace BusinessLine.Core.Application.Profiles.Commands.DeactivateProfile
{
    public sealed class DeactivateProfileCommand : IDeactivateProfileCommand
    {
        private readonly IProfileRepository _repository;
        private readonly IDateTimeService _dateTimeService;
        public DeactivateProfileCommand(IProfileRepository repository, IDateTimeService dateTimeService)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
        }

        public void Execute(DeactivateProfileModel model)
        {
            // Prerequisties
            var id = Guid.Parse(model.ActiveProfileId);

            var reason = TrimmedString.Create(model.Reason);

            var date = _dateTimeService.GetCurrentUtcDateTime();

            // Command
            _repository
                 .Find(id)
                 .IfSome(active =>
                 {
                     var passive = active.Deactivate(date, reason);
                     _repository.Add(passive);
                     _repository.Delete(active);
                     _repository.Save();
                 });
        }
    }
}