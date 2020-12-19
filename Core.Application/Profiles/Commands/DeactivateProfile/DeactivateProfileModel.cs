using System;

namespace Core.Application.Profiles.Commands.DeactivateProfile
{
    public sealed class DeactivateProfileModel
    {
        public Guid ActiveProfileId { get; set; }
        public string Reason { get; set; }
    }
}
