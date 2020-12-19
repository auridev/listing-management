using Core.Application.Listings.Commands.RemoveFavorite;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Core.Application.UnitTests.Listings.Commands.RemoveFavorite
{
    public class RemoveFavoriteModelValidator_should
    {
        private readonly RemoveFavoriteModelValidator _sut;
        public RemoveFavoriteModelValidator_should()
        {
            _sut = new RemoveFavoriteModelValidator();
        }

        [Fact]
        public void have_validation_error_if_ListingId_is_not_valid()
        {
            var model = new RemoveFavoriteModel()
            {
                ListingId = Guid.Empty
            };

            var result = _sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(model => model.ListingId);
        }

        [Fact]
        public void not_have_any_validation_errors_if_all_properties_are_valid()
        {
            var model = new RemoveFavoriteModel()
            {
                ListingId = Guid.NewGuid()
            };

            var result = _sut.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
