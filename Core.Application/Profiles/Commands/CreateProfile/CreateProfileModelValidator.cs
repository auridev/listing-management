using FluentValidation;
using FluentValidation.Validators;

namespace Core.Application.Profiles.Commands.CreateProfile
{
    public class CreateProfileModelValidator : AbstractValidator<CreateProfileModel>
    {
        public CreateProfileModelValidator()
        {
            RuleFor(model => model.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress(EmailValidationMode.AspNetCoreCompatible)
                .MaximumLength(50);

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

            RuleFor(model => model.DistanceUnit)
               .NotNull()
               .NotEmpty()
               .Must(distanceUnit => distanceUnit == "m" || distanceUnit == "km");

            RuleFor(model => model.MassUnit)
                .NotNull()
                .NotEmpty()
                .Must(massUnit => massUnit == "kg" || massUnit == "lb");

            RuleFor(model => model.CurrencyCode)
                .NotNull()
                .NotEmpty()
                .Length(3);
        }
    }
}
