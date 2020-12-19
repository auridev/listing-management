using Core.Application.Listings.Commands.DeactivateActiveListing;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Core.Application.UnitTests.Listings.Commands.DeactivateActiveListing
{
    public class DeactivateActiveListingModelValidator_should
    {
        private readonly DeactivateActiveListingModelValidator _sut;
        public DeactivateActiveListingModelValidator_should()
        {
            _sut = new DeactivateActiveListingModelValidator();
        }

        [Fact]
        public void have_validation_error_if_ListingId_is_not_valid()
        {
            var model = new DeactivateActiveListingModel()
            {
                ListingId = Guid.Empty
            };

            var result = _sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(model => model.ListingId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(
            @"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
            bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb
            cccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccc
            dddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd
            eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee
            ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff")]
        public void have_validation_error_if_Reason_is_not_valid(string invalidReason)
        {

            var model = new DeactivateActiveListingModel()
            {
                Reason = invalidReason
            };

            var result = _sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(model => model.Reason);
        }

        [Fact]
        public void not_have_any_validation_errors_if_all_properties_are_valid()
        {
            var model = new DeactivateActiveListingModel()
            {
                ListingId = Guid.NewGuid(),
                Reason = "some valid reason"
            };

            var result = _sut.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
