using Core.Application.Profiles.Commands.DeactivateProfile;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Core.Application.UnitTests.Profiles.Commands.DeactivateProfile
{
    public class DeactivateProfileModelValidator_should
    {
        private readonly DeactivateProfileModelValidator _sut;

        public DeactivateProfileModelValidator_should()
        {
            _sut = new DeactivateProfileModelValidator();
        }

        [Fact]
        public void have_validation_error_if_ActiveProfileId_is_not_valid()
        {
            var model = new DeactivateProfileModel()
            {
                ActiveProfileId = Guid.Empty
            };

            var result = _sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(model => model.ActiveProfileId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(
            @"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
            bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb
            ccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccc
            ddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd
            eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee")]
        public void have_validation_error_if_Reason_is_not_valid(string invalidReason)
        {
            var model = new DeactivateProfileModel()
            {
                Reason = invalidReason
            };

            var result = _sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(model => model.Reason);
        }

        [Fact]
        public void not_have_any_validation_errors_if_all_properties_are_valid()
        {
            var model = new DeactivateProfileModel()
            {
                ActiveProfileId = Guid.NewGuid(),
                Reason = "some valid reason"
            };

            var result = _sut.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
