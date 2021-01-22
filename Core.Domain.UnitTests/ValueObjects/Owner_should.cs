using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class Owner_should
    {
        [Fact]
        public void have_UserId_property()
        {
            Owner
                .Create(Guid.NewGuid())
                .Right(owner => owner.UserId.Should().NotBeEmpty())
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Theory]
        [MemberData(nameof(InvalidValueData))]
        public void return_EiherLeft_with_proper_error_during_creation_if_argument_is_not_valid(Guid value)
        {
            Either<Error, Owner> eitherOwner = Owner.Create(value);

            eitherOwner.IsLeft.Should().BeTrue();
            eitherOwner
               .Right(_ => throw InvalidExecutionPath.Exception)
               .Left(error => error.Should().BeOfType<Error.Invalid>());
        }

        public static IEnumerable<object[]> InvalidValueData => new List<object[]>
        {
            new object[] { new Guid() },
            new object[] { default(Guid) },
            new object[] { Guid.Empty }
        };

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_Values_match()
        {
            var first = Owner.Create(new Guid("36b310ce-8c17-46d0-990c-f5cf4f85f307"));
            var second = Owner.Create(new Guid("36b310ce-8c17-46d0-990c-f5cf4f85f307"));

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_Values_match()
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
