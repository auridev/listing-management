using Core.Domain.ValueObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class Recipient_should
    {
        [Fact]
        public void have_UserId_property()
        {
            var recipient = Recipient.Create(Guid.NewGuid());

            recipient.UserId.Should().NotBeEmpty();
        }

        [Theory]
        [MemberData(nameof(InvalidArguments))]
        public void thrown_an_exception_during_creation_if_argument_is_not_valid(Guid value)
        {
            Action action = () => Recipient.Create(value);

            action.Should().Throw<ArgumentException>();
        }

        public static IEnumerable<object[]> InvalidArguments => new List<object[]>
        {
            new object[] { new Guid() },
            new object[] { default(Guid) },
            new object[] { Guid.Empty }
        };

        [Fact]
        public void be_treated_as_equal_using_generic_equals_method_if_values_match()
        {
            var first = Recipient.Create(new Guid("2cb731c0-2b8c-48b5-b005-6d6ef8906b9b"));
            var second = Recipient.Create(new Guid("2cb731c0-2b8c-48b5-b005-6d6ef8906b9b"));

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }


        [Fact]
        public void be_treated_as_equal_using_object_equals_method_if_values_match()
        {
            var first = (object)Recipient.Create(new Guid("2cb731c0-2b8c-48b5-b005-6d6ef8906b9b"));
            var second = (object)Recipient.Create(new Guid("2cb731c0-2b8c-48b5-b005-6d6ef8906b9b"));

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_values_match()
        {
            var first = Recipient.Create(new Guid("2cb731c0-2b8c-48b5-b005-6d6ef8906b9b"));
            var second = Recipient.Create(new Guid("2cb731c0-2b8c-48b5-b005-6d6ef8906b9b"));

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_values_dont_match()
        {
            var first = Recipient.Create(new Guid("2cb731c0-2b8c-48b5-b005-6d6ef8906b9b"));
            var second = Recipient.Create(new Guid("60712ad9-0855-4257-8bc5-7cb6f6dc5ee1"));

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
