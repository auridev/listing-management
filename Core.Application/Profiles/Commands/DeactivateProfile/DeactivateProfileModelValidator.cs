using FluentValidation;

namespace Core.Application.Profiles.Commands.DeactivateProfile
{
    public class DeactivateProfileModelValidator : AbstractValidator<DeactivateProfileModel>
    {
        public DeactivateProfileModelValidator()
        {
            RuleFor(model => model.ActiveProfileId)
                .NotEmpty();

            RuleFor(model => model.Reason)
                .NotNull()
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}
