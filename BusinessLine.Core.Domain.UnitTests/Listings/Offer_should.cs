using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Listings;
using FluentAssertions;
using LanguageExt;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Listings
{
    public class Offer_should
    {
        private static readonly Owner _owner = Owner.Create(Guid.NewGuid());
        private static readonly MonetaryValue _monetaryValue = MonetaryValue.Create(12.5M, CurrencyCode.Create("123"));
        private static readonly SeenDate _seenDate = SeenDate.Create(DateTimeOffset.UtcNow);

        [Fact]
        public void have_an_Id_property()
        {
            var offer = new Offer(Guid.NewGuid(), _owner, _monetaryValue, DateTimeOffset.UtcNow, _seenDate);

            offer.Id.Should().NotBeEmpty();
        }

        [Fact]
        public void thrown_an_exception_during_creation_if_Id_is_not_valid()
        {
            Action createAction = () => new Offer(Guid.Empty, _owner, _monetaryValue, DateTimeOffset.UtcNow, _seenDate);

            createAction.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void have_an_Owner_property()
        {
            var offer = new Offer(Guid.NewGuid(), _owner, _monetaryValue, DateTimeOffset.UtcNow, _seenDate);

            offer.Owner.Should().Be(_owner);
        }

        [Fact]
        public void have_a_MonetaryValue_property()
        {
            var offer = new Offer(Guid.NewGuid(), _owner, _monetaryValue, DateTimeOffset.UtcNow, _seenDate);

            offer.MonetaryValue.Should().Be(_monetaryValue);
        }

        [Fact]
        public void have_CreatedDate_property()
        {
            var offer = new Offer(Guid.NewGuid(), _owner, _monetaryValue, DateTimeOffset.UtcNow, _seenDate);

            offer.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
        }

        [Fact]
        public void have_SeenDate_property()
        {
            var offer = new Offer(Guid.NewGuid(), _owner, _monetaryValue, DateTimeOffset.UtcNow, _seenDate);

            offer.SeenDate.Value.Should().BeCloseTo(DateTimeOffset.UtcNow, 5_000);
        }

        public static IEnumerable<object[]> InvalidArguments => new List<object[]>
        {
            new object[] { Guid.Empty, _owner, _monetaryValue,  DateTimeOffset.UtcNow },
            new object[] { Guid.NewGuid(), null, _monetaryValue,  DateTimeOffset.UtcNow },
            new object[] { Guid.NewGuid(), _owner, null,  DateTimeOffset.UtcNow },
            new object[] { Guid.NewGuid(), _owner, _monetaryValue, default }
        };

        [Theory]
        [MemberData(nameof(InvalidArguments))]
        public void thrown_an_exception_during_creation_if_arguments_are_not_valid(Guid id, Owner owner, MonetaryValue monetaryValue, DateTimeOffset createdDate)
        {
            Action createAction = () => new Offer(id, owner, monetaryValue, createdDate, _seenDate);

            createAction.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_ids_match()
        {
            // arrange
            var id = Guid.NewGuid();
            var first = new Offer(id, _owner, _monetaryValue, DateTimeOffset.UtcNow, _seenDate);
            var second = new Offer(id, _owner, _monetaryValue, DateTimeOffset.UtcNow, _seenDate);

            // act
            var equals = first.Equals(second);

            // assert
            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_ids_match()
        {
            // arrange
            var id = Guid.NewGuid();
            var first = (object) new Offer(id, _owner, _monetaryValue, DateTimeOffset.UtcNow, _seenDate);
            var second = (object) new Offer(id, _owner, _monetaryValue, DateTimeOffset.UtcNow, _seenDate);

            // act
            var equals = first.Equals(second);

            // assert
            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_ids_match()
        {
            // arrange
            var id = Guid.NewGuid();
            var first = new Offer(id, _owner, _monetaryValue, DateTimeOffset.UtcNow, _seenDate);
            var second = new Offer(id, _owner, _monetaryValue, DateTimeOffset.UtcNow, _seenDate);

            // act
            var equals = (first == second);

            // assert
            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_ids_dont_match()
        {
            // arrange
            var first = new Offer(Guid.NewGuid(), _owner, _monetaryValue, DateTimeOffset.UtcNow, _seenDate);
            var second = new Offer(Guid.NewGuid(), _owner, _monetaryValue, DateTimeOffset.UtcNow, _seenDate);

            // act
            var nonEquals = (first != second);

            // assert
            nonEquals.Should().BeTrue();
        }

        [Fact]
        public void have_the_same_hashcode_as_an_equal_offer()
        {
            // arrange
            var id = Guid.NewGuid();
            var first = new Offer(id, _owner, _monetaryValue, DateTimeOffset.UtcNow, _seenDate);
            var second = new Offer(id, _owner, _monetaryValue, DateTimeOffset.UtcNow, _seenDate);

            // act
            var equals = (first == second);
            var firstCode = first.GetHashCode();
            var secondCode = second.GetHashCode();

            // assert
            equals.Should().BeTrue();
            firstCode.Should().Be(secondCode);
        }

        [Fact]
        public void be_markable_as_seen()
        {
            var offer = new Offer(Guid.NewGuid(), 
                _owner, 
                _monetaryValue, 
                DateTimeOffset.UtcNow, 
                SeenDate.Create(DateTimeOffset.UtcNow));

            offer.HasBeenSeen(_seenDate);

            offer.SeenDate.Value.Should().BeCloseTo(DateTimeOffset.UtcNow, 5_000);
        }
    }
}
