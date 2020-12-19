using Core.Application.Listings.Commands.MarkNewListingAsSuspicious;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Core.Application.UnitTests.Listings.Commands.MarkNewListingAsSuspicious
{
    public class MarkNewListingAsSuspiciousModelValidator_should
    {
        private readonly MarkNewListingAsSuspiciousModelValidator _sut;
        public MarkNewListingAsSuspiciousModelValidator_should()
        {
            _sut = new MarkNewListingAsSuspiciousModelValidator();
        }

        [Fact]
        public void have_validation_error_if_ListingId_is_not_valid()
        {
            var model = new MarkNewListingAsSuspiciousModel()
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

            var model = new MarkNewListingAsSuspiciousModel()
            {
                Reason = invalidReason
            };

            var result = _sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(model => model.Reason);
        }

        [Fact]
        public void not_have_any_validation_errors_if_all_properties_are_valid()
        {
            var model = new MarkNewListingAsSuspiciousModel()
            {
                ListingId = Guid.NewGuid(),
                Reason = "some valid reason"
            };

            var result = _sut.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
