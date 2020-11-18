using Core.Domain.Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class Lead_should
    {
        private readonly Lead _sut;
        public Lead_should()
        {
            _sut = Lead.Create(Owner.Create(Guid.NewGuid()),
                DateTimeOffset.UtcNow);
        }

        [Fact]
        public void have_UserInterested_property()
        {
            _sut.UserInterested.Should().NotBeNull();
        }

        [Fact]
        public void have_DetailsSeenOn_property()
        {
            _sut.DetailsSeenOn.Should().BeCloseTo(DateTimeOffset.UtcNow);
        }

        [Theory]
        [MemberData(nameof(InvalidAguments))]
        public void throw_an_exception_during_creation_if_values_are_not_valid(Owner owner, DateTimeOffset createdDate)
        {
            Action createAction = () => Lead.Create(owner, createdDate);

            createAction.Should().Throw<ArgumentNullException>();
        }

        public static IEnumerable<object[]> InvalidAguments => new List<object[]>
        {
            new object[] { Owner.Create(Guid.NewGuid()), default},
            new object[] { null, DateTimeOffset.UtcNow }
        };

        [Fact]
        public void be_treated_as_equal_by_generic_equals_if_values_match()
        {
            var owner = Owner.Create(Guid.NewGuid());
            var createdDate = DateTimeOffset.UtcNow;
            var first = Lead.Create(owner, createdDate);
            var second = Lead.Create(owner, createdDate);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }
        [Fact]
        public void be_treated_as_equal_by_object_equals_if_values_match()
        {
            var owner = Owner.Create(Guid.NewGuid());
            var createdDate = DateTimeOffset.UtcNow;
            var first = (object)Lead.Create(owner, createdDate);
            var second = (object)Lead.Create(owner, createdDate);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_by_equality_operator_if_values_match()
        {
            var owner = Owner.Create(Guid.NewGuid());
            var createdDate = DateTimeOffset.UtcNow;
            var first = Lead.Create(owner, createdDate);
            var second = Lead.Create(owner, createdDate);

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_by_not_equal_operator_if_values_dont_match()
        {
            var first = Lead.Create(Owner.Create(Guid.NewGuid()), DateTimeOffset.UtcNow);
            var second = Lead.Create(Owner.Create(Guid.NewGuid()), DateTimeOffset.UtcNow);

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }

    }
}
