using Core.Domain.Common;
using System;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class Owner_should
    {
        [Fact]
        public void have_UserId_property()
        {
            var owner = Owner.Create(Guid.NewGuid());

            owner.UserId.Should().NotBeEmpty();
        }

        [Theory]
        [MemberData(nameof(InvalidValueData))]
        public void thrown_an_exception_during_creation_if_value_is_not_valid(Guid value)
        {
            Action action = () => Owner.Create(value);

            action.Should().Throw<ArgumentException>();
        }

        public static IEnumerable<object[]> InvalidValueData => new List<object[]>
        { 
            new object[] { new Guid() },
            new object[] { default(Guid) },
            new object[] { Guid.Empty }
        };

        [Fact]
        public void be_treated_as_equal_using_Equals_method_if_Values_match()
        {
            var first = Owner.Create(new Guid("36b310ce-8c17-46d0-990c-f5cf4f85f307"));
            var second = Owner.Create(new Guid("36b310ce-8c17-46d0-990c-f5cf4f85f307"));

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Values_match()
        {
            var first = Owner.Create(new Guid("21ccf801-54eb-4dc9-aee1-80cb60d64f1b"));
            var second = Owner.Create(new Guid("21ccf801-54eb-4dc9-aee1-80cb60d64f1b"));

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Values_dont_match()
        {
            var first = Owner.Create(new Guid("f39b3752-271d-4e3e-ae90-cd7f12735d09"));
            var second = Owner.Create(new Guid("80ba4c1a-d9f2-4157-a1db-376ed0a0dddd"));

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
