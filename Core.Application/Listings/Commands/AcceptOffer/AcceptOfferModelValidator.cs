using FluentValidation;

namespace Core.Application.Listings.Commands.AcceptOffer
{
    public class AcceptOfferModelValidator : AbstractValidator<AcceptOfferModel>
    {
        public AcceptOfferModelValidator()
        {
            RuleFor(model => model.ListingId)
                .NotEmpty();
            RuleFor(model => model.OfferId)
                .NotEmpty();
        }
    }
}
