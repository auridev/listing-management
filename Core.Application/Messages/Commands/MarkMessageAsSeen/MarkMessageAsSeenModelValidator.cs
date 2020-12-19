using FluentValidation;

namespace Core.Application.Messages.Commands.MarkMessageAsSeen
{
    public class MarkMessageAsSeenModelValidator : AbstractValidator<MarkMessageAsSeenModel>
    {
        public MarkMessageAsSeenModelValidator()
        {
            RuleFor(model => model.MessageId)
                .NotEmpty();
        }
    }
}
