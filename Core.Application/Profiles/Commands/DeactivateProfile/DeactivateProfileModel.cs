namespace Core.Application.Profiles.Commands.DeactivateProfile
{
    public sealed class DeactivateProfileModel
    {
        public string ActiveProfileId { get; set; }
        public string Reason { get; set; }
    }
}
