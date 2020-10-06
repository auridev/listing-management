using BusinessLine.Core.Domain.Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class MessageBody_should
    {
        private readonly MessageBody _sut;
        private static readonly ICollection<TemplateParam> _params = new List<TemplateParam>()
        {
            TemplateParam.Create("{id}", "123"),
            TemplateParam.Create("{name}", "AAA")
        };
        private static readonly Template _template = Template.Create("message text");

        public MessageBody_should()
        {
            _sut = MessageBody.Create(_template, _params);
        }

        [Fact]
        public void have_Template_property()
        {
            _sut.Template.Should().Be(_template);
        }

        [Fact]
        public void have_Params_property()
        {
            _sut.Params.Should().BeEquivalentTo(_params);
        }

        [Fact]
        public void be_treated_as_equal_using_generic_equals_method_if_templates_match()
        {
            var first = MessageBody.Create(
                Template.Create("qwerty"),
                new TemplateParam[]
                {
                    TemplateParam.Create("{id}", "123")
                });
            var second = MessageBody.Create(
                Template.Create("qwerty"),
                new TemplateParam[]
                {
                    TemplateParam.Create("{name}", "___")
                });

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_equals_method_if_templates_match()
        {
            var first = (object)MessageBody.Create(
                Template.Create("qwerty"),
                new TemplateParam[]
                {
                    TemplateParam.Create("{id}", "123")
                });
            var second = (object)MessageBody.Create(
                Template.Create("qwerty"),
                new TemplateParam[]
                {
                    TemplateParam.Create("{name}", "___")
                });

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_templates_match()
        {
            var first = MessageBody.Create(
                Template.Create("qwerty"),
                new TemplateParam[]
                {
                    TemplateParam.Create("{id}", "123")
                });
            var second = MessageBody.Create(
                Template.Create("qwerty"),
                new TemplateParam[]
                {
                    TemplateParam.Create("{name}", "___")
                });

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_templates_dont_match()
        {
            var first = MessageBody.Create(
                Template.Create("qwerty"),
                new TemplateParam[]
                {
                    TemplateParam.Create("{id}", "123")
                });
            var second = MessageBody.Create(
                Template.Create("aaaaa"),
                new TemplateParam[]
                {
                    TemplateParam.Create("{id}", "123")
                });

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(InvalidArguments))]
        public void throw_an_exception_during_creation_if_value_is_not_valid(Template template, TemplateParam[] templateParams)
        {
            Action createAction = () => MessageBody.Create(template, templateParams);

            createAction.Should().Throw<ArgumentException>();
        }

        public static IEnumerable<object[]> InvalidArguments => new List<object[]>
        {
            new object[] { null, null },
            new object[] { null, _params },
        };
    }
}
