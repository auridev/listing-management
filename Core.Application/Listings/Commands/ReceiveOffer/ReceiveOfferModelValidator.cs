using FluentValidation;

namespace Core.Application.Listings.Commands.ReceiveOffer
{
    public class ReceiveOfferModelValidator : AbstractValidator<ReceiveOfferModel>
    {
        public ReceiveOfferModelValidator()
        {
            RuleFor(model => model.ListingId)
                .NotEmpty();

            RuleFor(model => model.Value)
                .GreaterThan(0);

            RuleFor(model => model.CurrencyCode)
                .NotNull()
                .NotEmpty()
                .Length(3);
        }
    }
}
