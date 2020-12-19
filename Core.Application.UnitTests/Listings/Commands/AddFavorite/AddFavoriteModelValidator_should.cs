using Core.Application.Listings.Commands.AddFavorite;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Core.Application.UnitTests.Listings.Commands.AddFavorite
{
    public class AddFavoriteModelValidator_should
    {
        private readonly AddFavoriteModelValidator _sut;
        public AddFavoriteModelValidator_should()
        {
            _sut = new AddFavoriteModelValidator();
        }

        [Fact]
        public void have_validation_error_if_ListingId_is_not_valid()
        {
            var model = new AddFavoriteModel()
            {
                ListingId = Guid.Empty
            };

            var result = _sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(model => model.ListingId);
        }

        [Fact]
        public void not_have_any_validation_errors_if_all_properties_are_valid()
        {
            var model = new AddFavoriteModel()
            {
                ListingId = Guid.NewGuid()
            };

            var result = _sut.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
