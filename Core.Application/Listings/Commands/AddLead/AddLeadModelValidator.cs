using FluentValidation;

namespace Core.Application.Listings.Commands.AddLead
{
    public class AddLeadModelValidator : AbstractValidator<AddLeadModel>
    {
        public AddLeadModelValidator()
        {
            RuleFor(model => model.ListingId)
                .NotEmpty();
        }
    }
}
