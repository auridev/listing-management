using Core.Application.Listings.Commands.ReceiveOffer;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Core.Application.UnitTests.Listings.Commands.ReceiveOffer
{
    public class ReceiveOfferModelValidator_should
    {
        private readonly ReceiveOfferModelValidator _sut;
        public ReceiveOfferModelValidator_should()
        {
            _sut = new ReceiveOfferModelValidator();
        }

        [Fact]
        public void have_validation_error_if_ListingId_is_not_valid()
        {
            var model = new ReceiveOfferModel()
            {
                ListingId = Guid.Empty
            };

            var result = _sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(model => model.ListingId);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void have_validation_error_if_Value_is_not_valid(decimal invalidValue)
        {
            var model = new ReceiveOfferModel()
            {
                Value = invalidValue
            };

            var result = _sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(model => model.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("aa")]
        [InlineData("bbbb")]
        public void have_validation_error_if_CurrencyCode_is_not_valid(string invalidCurrencyCode)
        {
            var model = new ReceiveOfferModel()
            {
                CurrencyCode = invalidCurrencyCode
            };

            var result = _sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(model => model.CurrencyCode);
        }

        [Fact]
        public void not_have_any_validation_errors_if_all_properties_are_valid()
        {
            var model = new ReceiveOfferModel()
            {
                ListingId = Guid.NewGuid(),
                Value = 2.4M,
                CurrencyCode = "aaa"
            };

            var result = _sut.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
