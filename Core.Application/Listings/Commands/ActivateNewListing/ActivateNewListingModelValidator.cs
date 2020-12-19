using FluentValidation;

namespace Core.Application.Listings.Commands.ActivateNewListing
{
    public class ActivateNewListingModelValidator : AbstractValidator<ActivateNewListingModel>
    {
        public ActivateNewListingModelValidator()
        {
            RuleFor(model => model.ListingId)
                .NotEmpty();
        }
    }
}
