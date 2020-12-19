using FluentValidation;

namespace Core.Application.Listings.Commands.MarkOfferAsSeen
{
    public class MarkOfferAsSeenModelValidator : AbstractValidator<MarkOfferAsSeenModel>
    {
        public MarkOfferAsSeenModelValidator()
        {
            RuleFor(model => model.ListingId)
                .NotEmpty();

            RuleFor(model => model.OfferId)
                .NotEmpty();
        }
    }
}
