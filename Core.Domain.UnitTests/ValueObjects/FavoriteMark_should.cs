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
    public class FavoriteMark_should
    {
        [Fact]
        public void have_FavoredBy_property()
        {
            FavoriteMark
                .Create(Guid.NewGuid(), DateTimeOffset.UtcNow)
                .Right(favorite => favorite.FavoredBy.Should().NotBeNull())
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_MarkedAsFavoriteOn_property()
        {
            FavoriteMark
                .Create(Guid.NewGuid(), DateTimeOffset.UtcNow)
                .Right(favorite => favorite.MarkedAsFavoriteOn.Should().BeCloseTo(DateTimeOffset.UtcNow))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Theory]
        [MemberData(nameof(InvalidAguments))]
        public void return_EiherLeft_with_proper_error_during_creation_if_arguments_are_not_valid(Guid userId, DateTimeOffset markedAsFavoriteOn)
        {
            Either<Error, FavoriteMark> eitherFavorite = FavoriteMark.Create(userId, markedAsFavoriteOn);

            eitherFavorite.IsLeft.Should().BeTrue();
            eitherFavorite
               .Right(_ => throw InvalidExecutionPath.Exception)
               .Left(error => error.Should().BeOfType<Error.Invalid>());
        }

        public static IEnumerable<object[]> InvalidAguments => new List<object[]>
        {
            new object[] { null,  DateTimeOffset.UtcNow },
            new object[] { Guid.NewGuid(), default}
        };

        [Fact]
        public void be_treated_as_equal_by_generic_equals_if_values_match()
        {
            var favoredById = Guid.NewGuid();
            var markedAsFavoriteOn = DateTimeOffset.Now;

            var first = FavoriteMark.Create(favoredById, markedAsFavoriteOn);
            var second = FavoriteMark.Create(favoredById, markedAsFavoriteOn);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }
        [Fact]
        public void be_treated_as_equal_by_object_equals_if_values_match()
        {
            var favoredById = Guid.NewGuid();
            var markedAsFavoriteOn = DateTimeOffset.Now;

            var first = (object) FavoriteMark.Create(favoredById, markedAsFavoriteOn);
            var second = (object) FavoriteMark.Create(favoredById, markedAsFavoriteOn);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_by_equality_operator_if_values_match()
        {
            var favoredById = Guid.NewGuid();
            var markedAsFavoriteOn = DateTimeOffset.Now;

            var first = FavoriteMark.Create(favoredById, markedAsFavoriteOn);
            var second = FavoriteMark.Create(favoredById, markedAsFavoriteOn);

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_by_not_equal_operator_if_values_dont_match()
        {
            var first = FavoriteMark.Create(Guid.NewGuid(), DateTimeOffset.Now);
            var second = FavoriteMark.Create(Guid.NewGuid(), DateTimeOffset.Now);

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
