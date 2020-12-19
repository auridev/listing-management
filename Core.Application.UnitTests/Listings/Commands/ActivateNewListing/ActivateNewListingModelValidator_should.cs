using Core.Application.Listings.Commands.ActivateNewListing;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Core.Application.UnitTests.Listings.Commands.ActivateNewListing
{
    public class ActivateNewListingModelValidator_should
    {
        private readonly ActivateNewListingModelValidator _sut;
        public ActivateNewListingModelValidator_should()
        {
            _sut = new ActivateNewListingModelValidator();
        }

        [Fact]
        public void have_validation_error_if_ListingId_is_not_valid()
        {
            var model = new ActivateNewListingModel()
            {
                ListingId = Guid.Empty
            };

            var result = _sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(model => model.ListingId);
        }

        [Fact]
        public void not_have_any_validation_errors_if_all_properties_are_valid()
        {
            var model = new ActivateNewListingModel()
            {
                ListingId = Guid.NewGuid()
            };

            var result = _sut.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
