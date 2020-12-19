using Core.Application.Listings.Commands.ActivateSuspiciousListing;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Core.Application.UnitTests.Listings.Commands.ActivateSuspiciousListing
{
    public class ActivateSuspiciousListingModelValidator_should
    {
        private readonly ActivateSuspiciousListingModelValidator _sut;
        public ActivateSuspiciousListingModelValidator_should()
        {
            _sut = new ActivateSuspiciousListingModelValidator();
        }

        [Fact]
        public void have_validation_error_if_ListingId_is_not_valid()
        {
            var model = new ActivateSuspiciousListingModel()
            {
                ListingId = Guid.Empty
            };

            var result = _sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(model => model.ListingId);
        }

        [Fact]
        public void not_have_any_validation_errors_if_all_properties_are_valid()
        {
            var model = new ActivateSuspiciousListingModel()
            {
                ListingId = Guid.NewGuid()
            };

            var result = _sut.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
