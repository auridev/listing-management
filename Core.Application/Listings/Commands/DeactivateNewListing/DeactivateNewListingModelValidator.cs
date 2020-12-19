using FluentValidation;

namespace Core.Application.Listings.Commands.DeactivateNewListing
{
    public class DeactivateNewListingModelValidator : AbstractValidator<DeactivateNewListingModel>
    {
        public DeactivateNewListingModelValidator()
        {
            RuleFor(model => model.ListingId)
                .NotEmpty();

            RuleFor(model => model.Reason)
                .NotNull()
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}
