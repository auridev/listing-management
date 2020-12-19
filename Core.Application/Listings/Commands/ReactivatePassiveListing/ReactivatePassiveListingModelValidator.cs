using FluentValidation;

namespace Core.Application.Listings.Commands.ReactivatePassiveListing
{
    public class ReactivatePassiveListingModelValidator : AbstractValidator<ReactivatePassiveListingModel>
    {
        public ReactivatePassiveListingModelValidator()
        {
            RuleFor(model => model.ListingId)
                .NotEmpty();
        }
    }
}
