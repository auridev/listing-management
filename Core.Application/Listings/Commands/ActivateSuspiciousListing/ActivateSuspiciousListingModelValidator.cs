using FluentValidation;

namespace Core.Application.Listings.Commands.ActivateSuspiciousListing
{
    public class ActivateSuspiciousListingModelValidator : AbstractValidator<ActivateSuspiciousListingModel>
    {
        public ActivateSuspiciousListingModelValidator()
        {
            RuleFor(model => model.ListingId)
                .NotEmpty();
        }
    }
}
