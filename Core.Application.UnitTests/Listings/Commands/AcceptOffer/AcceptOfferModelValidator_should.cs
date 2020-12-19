using Core.Application.Listings.Commands.AcceptOffer;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Core.Application.UnitTests.Listings.Commands.AcceptOffer
{
    public class AcceptOfferModelValidator_should
    {
        private readonly AcceptOfferModelValidator _sut;

        public AcceptOfferModelValidator_should()
        {
            _sut = new AcceptOfferModelValidator();
        }

        [Fact]
        public void have_validation_error_if_ListingId_is_not_valid()
        {
            var model = new AcceptOfferModel()
            {
                ListingId = new Guid(),
                OfferId = Guid.NewGuid()
            };

            var result = _sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(model => model.ListingId);
        }

        [Fact]
        public void have_validation_error_if_OfferId_is_not_valid()
        {
            var model = new AcceptOfferModel()
            {
                ListingId = Guid.NewGuid(),
                OfferId = new Guid()
            };

            var result = _sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(model => model.OfferId);
        }

        [Fact]
        public void not_have_any_validation_errors_if_all_properties_are_valid()
        {
            var model = new AcceptOfferModel()
            {
                ListingId = Guid.NewGuid(),
                OfferId = Guid.NewGuid()
            };

            var result = _sut.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
