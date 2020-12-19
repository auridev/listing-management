using Core.Application.Messages.Commands.MarkMessageAsSeen;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Core.Application.UnitTests.Messages.Commands.MarkMessageAsSeen
{
    public class MarkMessageAsSeenModelValidator_should
    {
        private readonly MarkMessageAsSeenModelValidator _sut;
        public MarkMessageAsSeenModelValidator_should()
        {
            _sut = new MarkMessageAsSeenModelValidator();
        }

        [Fact]
        public void have_validation_error_if_MessageId_is_not_valid()
        {
            var model = new MarkMessageAsSeenModel()
            {
                MessageId = Guid.Empty
            };

            var result = _sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(model => model.MessageId);
        }

        [Fact]
        public void not_have_any_validation_errors_if_all_properties_are_valid()
        {
            var model = new MarkMessageAsSeenModel()
            {
                MessageId = Guid.NewGuid()
            };

            var result = _sut.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
