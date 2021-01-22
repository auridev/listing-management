using Common.Helpers;
using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Listings
{
    public class SuspiciousListing_should : Listing_should
    {
        private readonly SuspiciousListing _sut = new SuspiciousListing(
            Guid.NewGuid(),
            _owner,
            _listingDetails,
            _contactDetails,
            _locationDetails,
            _geographicLocation,
            _createdDate,
            DateTimeOffset.UtcNow,
            TestValueObjectFactory.CreateTrimmedString("robot didnt understand description"));

        [Fact]
        public void have_an_MarkedAsSuspiciousAt_property()
        {
            _sut.MarkedAsSuspiciousAt.Should().BeCloseTo(DateTimeOffset.UtcNow);
        }

        [Fact]
        public void have_a_Reason_property()
        {
            _sut.Reason.ToString().Should().Be("robot didnt understand description");
        }

        [Fact]
        public void be_deactivatable()
        {
            // act
            Either<Error, PassiveListing> action = _sut.Deactivate(TestValueObjectFactory.CreateTrimmedString("wrong number"), DateTimeOffset.UtcNow);

            // assert
            action
                .Right(passiveListing =>
                {
                    passiveListing.Should().NotBeNull();
                    passiveListing.DeactivationDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
                    passiveListing.Reason.Value.Should().Be("wrong number");
                })
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        public static IEnumerable<object[]> ArgumentsForDeactivate => new List<object[]>
        {
            new object[] { TestValueObjectFactory.CreateTrimmedString("aaaa"), default },
            new object[] { null, DateTimeOffset.UtcNow }
        };

        [Theory]
        [MemberData(nameof(ArgumentsForDeactivate))]
        public void reject_to_be_deactivated_if_arguments_are_not_valid(TrimmedString reason, DateTimeOffset date)
        {
            Either<Error, PassiveListing> action = _sut.Deactivate(reason, date);

            action
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error => error.Should().BeOfType<Error.Invalid>());
        }

        [Fact]
        public void be_activatable()
        {
            // act
            Either<Error, ActiveListing> action = _sut.Activate(DateTimeOffset.UtcNow);

            // assert
            action
                .Right(activeListing =>
                {
                    activeListing.Should().NotBeNull();
                    activeListing.ExpirationDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
                })
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void reject_to_be_activated_if_expiration_date_is_not_valid()
        {
            Either<Error, ActiveListing> action = _sut.Activate(default);

            action
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error => error.Should().BeOfType<Error.Invalid>());
        }

        public static IEnumerable<object[]> InvalidArgumentsForSuspiciousListing => new List<object[]>
        {
            new object[] { default, TestValueObjectFactory.CreateTrimmedString("bbb") },
            new object[] { DateTimeOffset.Now, null }
        };

        [Theory]
        [MemberData(nameof(InvalidArgumentsForSuspiciousListing))]
        public void thrown_an_exception_during_creation_if_some_arguments_are_not_valid(DateTimeOffset markedAsSuspiciousAt, TrimmedString reason)
        {
            Action createAction = () => new SuspiciousListing(Guid.NewGuid(),
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _createdDate,
                markedAsSuspiciousAt,
                reason);

            createAction.Should().Throw<ArgumentNullException>();
        }
    }
}
