using Core.Application.Messages.Commands.SendMessage;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Core.Application.UnitTests.Messages.Commands.SendMessage
{
    public class SendMessageModelValidator_should
    {
        private readonly SendMessageModelValidator _sut;

        public SendMessageModelValidator_should()
        {
            _sut = new SendMessageModelValidator();
        }

        [Fact]
        public void have_validation_error_if_Recipient_is_not_valid()
        {
            var model = new SendMessageModel()
            {
                Recipient = Guid.Empty
            };

            var result = _sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(model => model.Recipient);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(
            @"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
            bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb
            cccccccccccccccccccccccccccccccccccccccccccccccccccc
            dddddddddddddddddddddddddddddddddddddddddddddddddddd")]
        public void have_validation_error_if_Subject_is_not_valid(string invalidSubject)
        {
            var model = new SendMessageModel()
            {
                Subject = invalidSubject
            };

            var result = _sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(model => model.Subject);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(
            @"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
            bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb
            cccccccccccccccccccccccccccccccccccccccccccccccccccc
            dddddddddddddddddddddddddddddddddddddddddddddddddddd
            eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee
            ffffffffffffffffffffffffffffffffffffffffffffffffffff
            gggggggggggggggggggggggggggggggggggggggggggggggggggg
            hhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh
            jjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjj
            iiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii
            kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk
            llllllllllllllllllllllllllllllllllllllllllllllllllll
            mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
            nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn
            oooooooooooooooooooooooooooooooooooooooooooooooooooo
            pppppppppppppppppppppppppppppppppppppppppppppppppppp")]
        public void have_validation_error_if_Body_is_not_valid(string invalidBody)
        {
            var model = new SendMessageModel()
            {
                Body = invalidBody
            };

            var result = _sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(model => model.Body);
        }

        [Fact]
        public void not_have_any_validation_errors_if_all_properties_are_valid()
        {
            var model = new SendMessageModel()
            {
                Recipient = Guid.NewGuid(),
                Subject = "valid subject",
                Body = "valid body"
            };

            var result = _sut.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
