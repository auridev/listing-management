using Core.Domain.Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class FavoriteMark_should
    {
        private readonly FavoriteMark _sut;
        public FavoriteMark_should()
        {
            _sut = FavoriteMark.Create(Owner.Create(Guid.NewGuid()), DateTimeOffset.UtcNow);
        }

        [Fact]
        public void have_FavoredBy_property()
        {
            _sut.FavoredBy.Should().NotBeNull();
        }

        [Fact]
        public void have_MarkedAsFavoriteOn_property()
        {
            _sut.MarkedAsFavoriteOn.Should().BeCloseTo(DateTimeOffset.UtcNow);
        }

        [Theory]
        [MemberData(nameof(InvalidAguments))]
        public void throw_an_exception_during_creation_if_values_are_not_valid(Owner favoredBy, DateTimeOffset markedAsFavoriteOn)
        {
            Action createAction = () => FavoriteMark.Create(favoredBy, markedAsFavoriteOn);

            createAction.Should().Throw<ArgumentException>();
        }

        public static IEnumerable<object[]> InvalidAguments => new List<object[]>
        {
            new object[] { null,  DateTimeOffset.UtcNow },
            new object[] { Owner.Create(Guid.NewGuid()), default}
        };

        [Fact]
        public void be_treated_as_equal_by_generic_equals_if_values_match()
        {
            var favoredBy = Owner.Create(Guid.NewGuid());
            var markedAsFavoriteOn = DateTimeOffset.Now;

            var first = FavoriteMark.Create(favoredBy, markedAsFavoriteOn);
            var second = FavoriteMark.Create(favoredBy, markedAsFavoriteOn);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }
        [Fact]
        public void be_treated_as_equal_by_object_equals_if_values_match()
        {
            var favoredBy = Owner.Create(Guid.NewGuid());
            var markedAsFavoriteOn = DateTimeOffset.Now;

            var first = (object)FavoriteMark.Create(favoredBy, markedAsFavoriteOn);
            var second = (object)FavoriteMark.Create(favoredBy, markedAsFavoriteOn);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_by_equality_operator_if_values_match()
        {
            var favoredBy = Owner.Create(Guid.NewGuid());
            var markedAsFavoriteOn = DateTimeOffset.Now;

            var first = FavoriteMark.Create(favoredBy, markedAsFavoriteOn);
            var second = FavoriteMark.Create(favoredBy, markedAsFavoriteOn);

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_by_not_equal_operator_if_values_dont_match()
        {
            var first = FavoriteMark.Create(Owner.Create(Guid.NewGuid()),
               DateTimeOffset.Now);
            var second = FavoriteMark.Create(Owner.Create(Guid.NewGuid()),
               DateTimeOffset.Now);

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
