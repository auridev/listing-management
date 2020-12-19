using FluentValidation;

namespace Core.Application.Listings.Commands.MarkNewListingAsSuspicious
{
    public class MarkNewListingAsSuspiciousModelValidator : AbstractValidator<MarkNewListingAsSuspiciousModel>
    {
        public MarkNewListingAsSuspiciousModelValidator()
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
