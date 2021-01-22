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
    public class Lead_should
    {
        [Fact]
        public void have_UserInterested_property()
        {
            Lead
                .Create(Guid.NewGuid(), DateTimeOffset.UtcNow)
                .Right(lead => lead.UserInterested.Should().NotBeNull())
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_DetailsSeenOn_property()
        {
            Lead
                .Create(Guid.NewGuid(), DateTimeOffset.UtcNow)
                .Right(lead => lead.DetailsSeenOn.Should().BeCloseTo(DateTimeOffset.UtcNow))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Theory]
        [MemberData(nameof(InvalidAguments))]
        public void return_EiherLeft_with_proper_error_during_creation_if_arguments_are_not_valid(Guid owner, DateTimeOffset createdDate)
        {
            Either<Error, Lead> eitherLead = Lead.Create(owner, createdDate);

            eitherLead.IsLeft.Should().BeTrue();
            eitherLead
               .Right(_ => throw InvalidExecutionPath.Exception)
               .Left(error => error.Should().BeOfType<Error.Invalid>());
        }

        public static IEnumerable<object[]> InvalidAguments => new List<object[]>
        {
            new object[] { Guid.NewGuid(), default},
            new object[] { null, DateTimeOffset.UtcNow }
        };

        [Fact]
        public void be_treated_as_equal_by_generic_equals_if_values_match()
        {
            var ownerId = Guid.NewGuid();
            var createdDate = DateTimeOffset.UtcNow;
            var first = Lead.Create(ownerId, createdDate);
            var second = Lead.Create(ownerId, createdDate);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }
        [Fact]
        public void be_treated_as_equal_by_object_equals_if_values_match()
        {
            var ownerId = Guid.NewGuid();
            var createdDate = DateTimeOffset.UtcNow;
            var first = (object) Lead.Create(ownerId, createdDate);
            var second = (object) Lead.Create(ownerId, createdDate);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_by_equality_operator_if_values_match()
        {
            var ownerId = Guid.NewGuid();
            var createdDate = DateTimeOffset.UtcNow;
            var first = Lead.Create(ownerId, createdDate);
            var second = Lead.Create(ownerId, createdDate);

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_by_not_equal_operator_if_values_dont_match()
        {
            var first = Lead.Create(Guid.NewGuid(), DateTimeOffset.UtcNow);
            var second = Lead.Create(Guid.NewGuid(), DateTimeOffset.UtcNow);

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
