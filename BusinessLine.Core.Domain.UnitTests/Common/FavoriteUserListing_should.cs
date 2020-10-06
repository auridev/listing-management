using BusinessLine.Core.Domain.Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class FavoriteUserListing_should
    {
        private readonly FavoriteUserListing _sut;
        public FavoriteUserListing_should()
        {
            _sut = FavoriteUserListing.Create(Guid.NewGuid(), 
                Owner.Create(Guid.NewGuid()), 
                Guid.NewGuid());
        }

        [Fact]
        public void have_Id_property()
        {
            _sut.Id.Should().NotBeEmpty();
        }

        [Fact]
        public void have_owner_property()
        {
            _sut.Owner.Should().NotBeNull();
        }

        [Fact]
        public void have_listing_id_property()
        {
            _sut.ListingId.Should().NotBeEmpty();
        }

        [Theory]
        [MemberData(nameof(InvalidAguments))]
        public void throw_an_exception_during_creation_if_values_are_not_valid(Guid id, Owner owner, Guid listingId)
        {
            Action createAction = () => FavoriteUserListing.Create(id, owner, listingId);

            createAction.Should().Throw<ArgumentException>();
        }

        public static IEnumerable<object[]> InvalidAguments => new List<object[]>
        {
            new object[] { Guid.NewGuid(), null, Guid.NewGuid() },
            new object[] { Guid.NewGuid(), Owner.Create(Guid.NewGuid()), default},
            new object[] { default, Owner.Create(Guid.NewGuid()), Guid.NewGuid() }
        };

        [Fact]
        public void be_treated_as_equal_by_generic_equals_if_values_match()
        {
            var id = Guid.NewGuid();
            var owner = Owner.Create(Guid.NewGuid());
            var listingId = Guid.NewGuid();
            var first = FavoriteUserListing.Create(id, owner, listingId);
            var second = FavoriteUserListing.Create(id, owner, listingId);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }
        [Fact]
        public void be_treated_as_equal_by_object_equals_if_values_match()
        {
            var id = Guid.NewGuid();
            var owner = Owner.Create(Guid.NewGuid());
            var listingId = Guid.NewGuid();
            var first = (object) FavoriteUserListing.Create(id, owner, listingId);
            var second = (object) FavoriteUserListing.Create(id, owner, listingId);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_by_equality_operator_if_values_match()
        {
            var id = Guid.NewGuid();
            var owner = Owner.Create(Guid.NewGuid());
            var listingId = Guid.NewGuid();
            var first = FavoriteUserListing.Create(id, owner, listingId);
            var second = FavoriteUserListing.Create(id, owner, listingId);

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_by_not_equal_operator_if_values_dont_match()
        {
            var first = FavoriteUserListing.Create(Guid.NewGuid(), 
                Owner.Create(Guid.NewGuid()), 
                Guid.NewGuid());
            var second = FavoriteUserListing.Create(Guid.NewGuid(), 
                Owner.Create(Guid.NewGuid()), 
                Guid.NewGuid());

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
