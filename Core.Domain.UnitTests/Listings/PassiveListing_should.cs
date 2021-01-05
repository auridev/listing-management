﻿using Core.Domain.ValueObjects;
using Core.Domain.Listings;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;
using Common.Helpers;

namespace BusinessLine.Core.Domain.UnitTests.Listings
{
    public class PassiveListing_should : Listing_should
    {
        [Fact(Skip = "while refactoring")]
        public void have_an_DeactivationDate_property()
        {
            var passiveListing = new PassiveListing(Guid.NewGuid(), 
                _owner, 
                _listingDetails, 
                _contactDetails,
                _locationDetails, 
                _geographicLocation,
                _createdDate,
                DateTimeOffset.UtcNow.AddDays(-3), 
                TrimmedString.Create("something bad").ToUnsafeRight());

            passiveListing.DeactivationDate.Should().BeCloseTo(DateTimeOffset.UtcNow.AddDays(-3));
        }

        [Fact(Skip = "this should be in command logic")]
        public void thrown_an_exception_during_creation_if_expiration_date_has_passed()
        {
            Action createAction = () => new PassiveListing(Guid.NewGuid(), 
                _owner, 
                _listingDetails, 
                _contactDetails,
                _locationDetails, 
                _geographicLocation,
                _createdDate,
                DateTimeOffset.UtcNow.AddDays(1), 
                TrimmedString.Create("aaaaa").ToUnsafeRight());

            createAction.Should().Throw<ArgumentException>();
        }

        public static IEnumerable<object[]> InvalidArgumentsForPassiveListing => new List<object[]>
        {
            new object[] { default, TrimmedString.Create("aaa") },
            new object[] { DateTimeOffset.Now, null }
        };

        [Theory(Skip = "aaaaaa")]
        [MemberData(nameof(InvalidArgumentsForPassiveListing))]
        public void thrown_an_exception_during_creation_if_some_arguments_are_not_valid(DateTimeOffset closedOn, TrimmedString reason)
        {
            Action createAction = () => new PassiveListing(Guid.NewGuid(),
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _createdDate,
                closedOn,
                reason);

            createAction.Should().Throw<ArgumentNullException>();
        }

        [Fact(Skip = "while refactoring")]
        public void have_a_Reason_property()
        {
            var passiveListing = new PassiveListing(Guid.NewGuid(), 
                _owner, 
                _listingDetails, 
                _contactDetails,
                _locationDetails, 
                _geographicLocation,
                _createdDate,
                DateTimeOffset.UtcNow.AddDays(-111), 
                TrimmedString.Create("xx xx").ToUnsafeRight());

            passiveListing.Reason.ToString().Should().Be("xx xx");
        }

        [Fact(Skip = "while refactoring")]
        public void be_reactivatable()
        {
            // arrange
            var passiveListing = new PassiveListing(
                Guid.NewGuid(),
                _owner, 
                _listingDetails, 
                _contactDetails,
                _locationDetails, 
                _geographicLocation,
                _createdDate,
                DateTimeOffset.UtcNow.AddDays(-10), 
                TrimmedString.Create("incorrect details").ToUnsafeRight());

            // act
            ActiveListing activeListing = passiveListing.Reactivate(DateTimeOffset.UtcNow.AddDays(90));

            // assert
            activeListing.Should().NotBeNull();
            activeListing.ExpirationDate.Should().BeCloseTo(DateTimeOffset.UtcNow.AddDays(90));
        }
    }
}
