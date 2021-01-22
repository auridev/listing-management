using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class SeenDate_should
    {
        [Fact]
        public void have_a_Value_property()
        {
            SeenDate
                .Create(DateTimeOffset.Now.AddDays(-1))
                .Right(seenDate => seenDate.Value.Should().BeCloseTo(DateTimeOffset.Now.AddDays(-1)))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void return_EiherLeft_with_proper_error_during_creation_if_argument_is_not_valid()
        {
            Either<Error, SeenDate> eitherSeenDate = SeenDate.Create(default);

            eitherSeenDate.IsLeft.Should().BeTrue();
            eitherSeenDate
               .Right(_ => throw InvalidExecutionPath.Exception)
               .Left(error => error.Should().BeOfType<Error.Invalid>());
        }

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_DateTimeOffset_values_match()
        {
            var dateTimeOffset = DateTimeOffset.UtcNow;
            var first = SeenDate.Create(dateTimeOffset);
            var second = SeenDate.Create(dateTimeOffset);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_DateTimeOffset_values_match()
        {
            var dateTimeOffset = DateTimeOffset.UtcNow;
            var first = (object)SeenDate.Create(dateTimeOffset);
            var second = (object)SeenDate.Create(dateTimeOffset);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_DateTimeOffset_values_match()
        {
            var dateTimeOffset = DateTimeOffset.UtcNow;
            var first = SeenDate.Create(dateTimeOffset);
            var second = SeenDate.Create(dateTimeOffset);

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_DateTimeOffset_values_dont_match()
        {
            var first = SeenDate.Create(DateTimeOffset.Now.AddDays(-2));
            var second = SeenDate.Create(DateTimeOffset.Now.AddDays(-3));

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
