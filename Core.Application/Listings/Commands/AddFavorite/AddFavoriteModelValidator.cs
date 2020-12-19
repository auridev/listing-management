using FluentValidation;

namespace Core.Application.Listings.Commands.AddFavorite
{
    public class AddFavoriteModelValidator : AbstractValidator<AddFavoriteModel>
    {
        public AddFavoriteModelValidator()
        {
            RuleFor(model => model.ListingId)
                .NotEmpty();
        }
    }
}
