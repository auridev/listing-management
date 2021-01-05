using Core.Domain.ValueObjects;
using Core.Domain.Listings;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;
using Common.Helpers;

namespace BusinessLine.Core.Domain.UnitTests.Listings
{
    public class SuspiciousListing_should : Listing_should
    {
        private readonly SuspiciousListing _sut;

        public SuspiciousListing_should()
        {
            _sut = new SuspiciousListing(Guid.NewGuid(),
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _createdDate,
                DateTimeOffset.UtcNow,
                TrimmedString.Create("robot didnt understand description").ToUnsafeRight());
        }

        [Fact(Skip = "while refactoring")]
        public void have_an_MarkedAsSuspiciousAt_property()
        {
            _sut.MarkedAsSuspiciousAt.Should().BeCloseTo(DateTimeOffset.UtcNow);
        }

        [Fact(Skip = "while refactoring")]
        public void have_a_Reason_property()
        {
            _sut.Reason.ToString().Should().Be("robot didnt understand description");
        }

        [Fact(Skip = "while refactoring")]
        public void be_deactivatable()
        {
            // act
            PassiveListing passiveListing = _sut.Deactivate(TrimmedString.Create("wrong number").ToUnsafeRight(), DateTimeOffset.UtcNow);

            // assert
            passiveListing.Should().NotBeNull();
            passiveListing.DeactivationDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
            passiveListing.Reason.Value.Should().Be("wrong number");
        }

        [Fact(Skip = "while refactoring")]
        public void be_activatable()
        {
            // act
            ActiveListing activeListing = _sut.Activate(DateTimeOffset.UtcNow);

            // assert
            activeListing.Should().NotBeNull();
            activeListing.ExpirationDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
        }

        public static IEnumerable<object[]> InvalidArgumentsForSuspiciousListing => new List<object[]>
        {
            new object[] { default, TrimmedString.Create("bbb") },
            new object[] { DateTimeOffset.Now, null }
        };

        [Theory(Skip = "while refactoring")]
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
