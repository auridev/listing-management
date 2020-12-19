using Core.Application.Profiles.Commands.MarkProfileAsIntroduced;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Core.Application.UnitTests.Profiles.Commands.MarkProfileAsIntroduced
{
    public class MarkProfileAsIntroducedModelValidator_should
    {
        private readonly MarkProfileAsIntroducedModelValidator _sut;
        public MarkProfileAsIntroducedModelValidator_should()
        {
            _sut = new MarkProfileAsIntroducedModelValidator();
        }

        [Fact]
        public void have_validation_error_if_ProfileId_is_not_valid()
        {
            var model = new MarkProfileAsIntroducedModel()
            {
                ProfileId = Guid.Empty
            };

            var result = _sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(model => model.ProfileId);
        }

        [Fact]
        public void not_have_any_validation_errors_if_all_properties_are_valid()
        {
            var model = new MarkProfileAsIntroducedModel()
            {
                ProfileId = Guid.NewGuid()
            };

            var result = _sut.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
