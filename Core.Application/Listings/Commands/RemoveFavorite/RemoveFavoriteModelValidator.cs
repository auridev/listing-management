using FluentValidation;

namespace Core.Application.Listings.Commands.RemoveFavorite
{
    public class RemoveFavoriteModelValidator : AbstractValidator<RemoveFavoriteModel>
    {
        public RemoveFavoriteModelValidator()
        {
            RuleFor(model => model.ListingId)
                .NotEmpty();
        }
    }
}
