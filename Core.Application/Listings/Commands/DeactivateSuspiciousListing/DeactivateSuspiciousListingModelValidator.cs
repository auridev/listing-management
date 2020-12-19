using FluentValidation;

namespace Core.Application.Listings.Commands.DeactivateSuspiciousListing
{
    public class DeactivateSuspiciousListingModelValidator : AbstractValidator<DeactivateSuspiciousListingModel>
    {
        public DeactivateSuspiciousListingModelValidator()
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
