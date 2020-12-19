using FluentValidation;

namespace Core.Application.Messages.Commands.SendMessage
{
    public class SendMessageModelValidator : AbstractValidator<SendMessageModel>
    {
        public SendMessageModelValidator()
        {
            RuleFor(model => model.Recipient)
               .NotEmpty();

            RuleFor(model => model.Subject)
                .NotNull()
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(model => model.Body)
                .NotNull()
                .NotEmpty()
                .MaximumLength(1000);
        }
    }
}
