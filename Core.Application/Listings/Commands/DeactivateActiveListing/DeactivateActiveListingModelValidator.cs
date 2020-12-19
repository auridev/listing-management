using FluentValidation;

namespace Core.Application.Listings.Commands.DeactivateActiveListing
{
    public class DeactivateActiveListingModelValidator : AbstractValidator<DeactivateActiveListingModel>
    {
        public DeactivateActiveListingModelValidator()
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
