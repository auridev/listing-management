using Core.Domain.ValueObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Offers
{
    public abstract class Offer_should
    {
        private static readonly Owner _owner = TestValueObjectFactory.CreateOwner(Guid.NewGuid());
        private static readonly MonetaryValue _monetaryValue = TestValueObjectFactory.CreateMonetaryValue(12.5M, "123");

        [Fact]
        public void have_an_Id_property()
        {
            var offer = new OfferTestFake(Guid.NewGuid(), _owner, _monetaryValue, DateTimeOffset.UtcNow);

            offer.Id.Should().NotBeEmpty();
        }

        [Fact]
        public void have_an_Owner_property()
        {
            var offer = new OfferTestFake(Guid.NewGuid(), _owner, _monetaryValue, DateTimeOffset.UtcNow);

            offer.Owner.Should().Be(_owner);
        }

        [Fact]
        public void have_a_MonetaryValue_property()
        {
            var offer = new OfferTestFake(Guid.NewGuid(), _owner, _monetaryValue, DateTimeOffset.UtcNow);

            offer.MonetaryValue.Should().Be(_monetaryValue);
        }

        [Fact]
        public void have_CreatedDate_property()
        {
            var offer = new OfferTestFake(Guid.NewGuid(), _owner, _monetaryValue, DateTimeOffset.UtcNow);

            offer.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
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
            Action createAction = () => new OfferTestFake(id, owner, monetaryValue, createdDate);

            createAction.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_ids_match()
        {
            // arrange
            var id = Guid.NewGuid();
            var first = new OfferTestFake(id, _owner, _monetaryValue, DateTimeOffset.UtcNow);
            var second = new OfferTestFake(id, _owner, _monetaryValue, DateTimeOffset.UtcNow);

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
            var first = (object)new OfferTestFake(id, _owner, _monetaryValue, DateTimeOffset.UtcNow);
            var second = (object)new OfferTestFake(id, _owner, _monetaryValue, DateTimeOffset.UtcNow);

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
            var first = new OfferTestFake(id, _owner, _monetaryValue, DateTimeOffset.UtcNow);
            var second = new OfferTestFake(id, _owner, _monetaryValue, DateTimeOffset.UtcNow);

            // act
            var equals = (first == second);

            // assert
            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_ids_dont_match()
        {
            // arrange
            var first = new OfferTestFake(Guid.NewGuid(), _owner, _monetaryValue, DateTimeOffset.UtcNow);
            var second = new OfferTestFake(Guid.NewGuid(), _owner, _monetaryValue, DateTimeOffset.UtcNow);

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
            var first = new OfferTestFake(id, _owner, _monetaryValue, DateTimeOffset.UtcNow);
            var second = new OfferTestFake(id, _owner, _monetaryValue, DateTimeOffset.UtcNow);

            // act
            var equals = (first == second);
            var firstCode = first.GetHashCode();
            var secondCode = second.GetHashCode();

            // assert
            equals.Should().BeTrue();
            firstCode.Should().Be(secondCode);
        }
    }
}
