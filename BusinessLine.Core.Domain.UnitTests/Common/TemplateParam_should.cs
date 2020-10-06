using BusinessLine.Core.Domain.Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class TemplateParam_should
    {
        private readonly TemplateParam _sut;

        public TemplateParam_should()
        {
            _sut = TemplateParam.Create("tag1", "value1");
        }

        [Fact]
        public void have_Tag_property()
        {
            _sut.Tag.Should().Be("tag1");
        }

        [Fact]
        public void have_Value_property()
        {
            _sut.Value.Should().Be("value1");
        }

        [Fact]
        public void be_treated_as_equal_using_generic_equals_method_if_values_match()
        {
            var first = TemplateParam.Create("tag1", "value1");
            var second = TemplateParam.Create("tag1", "value1");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_equals_method_if_values_match()
        {
            var first = (object)TemplateParam.Create("tag1", "value1");
            var second = (object)TemplateParam.Create("tag1", "value1");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_values_match()
        {
            var first = TemplateParam.Create("tag1", "value1");
            var second = TemplateParam.Create("tag1", "value1");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_values_dont_match()
        {
            var first = TemplateParam.Create("tag1", "value1");
            var second = TemplateParam.Create("tag2", "value2");

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(InvalidArguments))]
        public void throw_an_exception_during_creation_if_value_is_not_valid(string tag, string value)
        {
            Action createAction = () => TemplateParam.Create(tag, value);

            createAction.Should().Throw<ArgumentException>();
        }

        public static IEnumerable<object[]> InvalidArguments => new List<object[]>
        {
            new object[] { null, null },
            new object[] { string.Empty, string.Empty },
            new object[] { "", "" },
            new object[] { default, default }
        };
    }
}
