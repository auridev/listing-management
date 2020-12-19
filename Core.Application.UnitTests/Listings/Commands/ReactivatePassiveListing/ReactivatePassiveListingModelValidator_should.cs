using Core.Application.Listings.Commands.ReactivatePassiveListing;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Core.Application.UnitTests.Listings.Commands.ReactivatePassiveListing
{
    public class ReactivatePassiveListingModelValidator_should
    {
        private readonly ReactivatePassiveListingModelValidator _sut;
        public ReactivatePassiveListingModelValidator_should()
        {
            _sut = new ReactivatePassiveListingModelValidator();
        }

        [Fact]
        public void have_validation_error_if_ListingId_is_not_valid()
        {
            var model = new ReactivatePassiveListingModel()
            {
                ListingId = Guid.Empty
            };

            var result = _sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(model => model.ListingId);
        }

        [Fact]
        public void not_have_any_validation_errors_if_all_properties_are_valid()
        {
            var model = new ReactivatePassiveListingModel()
            {
                ListingId = Guid.NewGuid()
            };

            var result = _sut.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
