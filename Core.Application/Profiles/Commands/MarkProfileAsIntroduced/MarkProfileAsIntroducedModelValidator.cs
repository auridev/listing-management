using FluentValidation;

namespace Core.Application.Profiles.Commands.MarkProfileAsIntroduced
{
    public class MarkProfileAsIntroducedModelValidator : AbstractValidator<MarkProfileAsIntroducedModel>
    {
        public MarkProfileAsIntroducedModelValidator()
        {
            RuleFor(model => model.ProfileId)
                .NotEmpty();
        }
    }
}
