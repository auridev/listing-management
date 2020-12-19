using Core.Application.Listings.Commands.MarkOfferAsSeen;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Core.Application.UnitTests.Listings.Commands.MarkOfferAsSeen
{
    public class MarkOfferAsSeenModelValidator_should
    {
        private readonly MarkOfferAsSeenModelValidator _sut;
        public MarkOfferAsSeenModelValidator_should()
        {
            _sut = new MarkOfferAsSeenModelValidator();
        }

        [Fact]
        public void have_validation_error_if_ListingId_is_not_valid()
        {
            var model = new MarkOfferAsSeenModel()
            {
                ListingId = Guid.Empty
            };

            var result = _sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(model => model.ListingId);
        }

        [Fact]
        public void have_validation_error_if_OfferId_is_not_valid()
        {
            var model = new MarkOfferAsSeenModel()
            {
                OfferId = Guid.Empty
            };

            var result = _sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(model => model.OfferId);
        }

        [Fact]
        public void not_have_any_validation_errors_if_all_properties_are_valid()
        {
            var model = new MarkOfferAsSeenModel()
            {
                ListingId = Guid.NewGuid(),
                OfferId = Guid.NewGuid()
            };

            var result = _sut.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
