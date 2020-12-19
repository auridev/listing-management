using FluentValidation;
using System.Linq;

namespace Core.Application.Listings.Commands.CreateNewListing
{
    public class CreateNewListingModelValidator : AbstractValidator<CreateNewListingModel>
    {
        public CreateNewListingModelValidator()
        {
            //TODO add validation for images


            RuleFor(model => model.Title)
                .NotNull()
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(model => model.MaterialTypeId)
                .Must(type => new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90 }.Contains(type));

            RuleFor(model => model.Weight)
                .GreaterThan(0);

            RuleFor(model => model.MassUnit)
                .NotNull()
                .NotEmpty()
                .Must(massUnit => massUnit == "kg" || massUnit == "lb");

            RuleFor(model => model.Description)
                .NotNull()
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(model => model.FirstName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(model => model.LastName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(model => model.Company)
                .MaximumLength(50);

            RuleFor(model => model.Phone)
                .NotNull()
                .NotEmpty()
                .MaximumLength(25);

            RuleFor(model => model.CountryCode)
                .NotNull()
                .NotEmpty()
                .MaximumLength(5);

            RuleFor(model => model.State)
                .MaximumLength(25);

            RuleFor(model => model.City)
                .NotNull()
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(model => model.PostCode)
                .NotNull()
                .NotEmpty()
                .MaximumLength(15);

            RuleFor(model => model.Address)
                .NotNull()
                .NotEmpty()
                .MaximumLength(250);

            RuleFor(model => model.Latitude)
                .GreaterThan(-85.05112878D)
                .LessThan(85.05112878D);

            RuleFor(model => model.Longitude)
                .GreaterThan(-180D)
                .LessThan(180D);
        }
    }
}
