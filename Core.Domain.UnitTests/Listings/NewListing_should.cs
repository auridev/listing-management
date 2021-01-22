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
    public class NewListing_should : Listing_should
    {
        private readonly NewListing _sut;

        public NewListing_should()
        {
            _sut = new NewListing(Guid.NewGuid(),
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                DateTimeOffset.UtcNow);
        }

        [Fact]
        public void be_deactivatble()
        {
            // arrange
            TrimmedString reason = TestValueObjectFactory.CreateTrimmedString("somethings not right");

            // act
            Either<Error, PassiveListing> deactivateAction = _sut.Deactivate(reason, DateTimeOffset.UtcNow);

            // assert
            deactivateAction
                .Right(passiveListing =>
                {
                    passiveListing.Should().NotBeNull();
                    passiveListing.DeactivationDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
                    passiveListing.Reason.Value.Should().Be("somethings not right");
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
            // arrange
            DateTimeOffset expirationDate = DateTimeOffset.Now.AddDays(1);

            // act
            Either<Error, ActiveListing> activeAction = _sut.Activate(expirationDate);

            // assert
            activeAction
                .Right(activeListing =>
                {
                    activeListing.Should().NotBeNull();
                    activeListing.ExpirationDate.Should().BeCloseTo(DateTimeOffset.UtcNow.AddDays(1));
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

        [Fact]
        public void be_markable_as_suspicious()
        {
            // arrange
            TrimmedString reason = TestValueObjectFactory.CreateTrimmedString("for test");

            // act
            Either<Error, SuspiciousListing> action = _sut.MarkAsSuspicious(DateTimeOffset.UtcNow, reason);

            action
                .Right(suspiciousListing =>
                {
                    suspiciousListing.Should().NotBeNull();
                    suspiciousListing.MarkedAsSuspiciousAt.Should().BeCloseTo(DateTimeOffset.UtcNow);
                    suspiciousListing.Reason.Value.Should().Be("for test");
                })
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        public static IEnumerable<object[]> ArgumentsForMarkAsSuspicious => new List<object[]>
        {
            new object[] { default, TestValueObjectFactory.CreateTrimmedString("vienas du trys") },
            new object[] { DateTimeOffset.UtcNow, null }
        };

        [Theory]
        [MemberData(nameof(ArgumentsForMarkAsSuspicious))]
        public void reject_to_be_marked_as_suspicious_if_arguments_are_not_valid(DateTimeOffset date, TrimmedString reason)
        {
            Either<Error, SuspiciousListing> action = _sut.MarkAsSuspicious(date, reason);

            action
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error => error.Should().BeOfType<Error.Invalid>());
        }

        [Fact]
        public void thrown_an_exception_during_creation_if_some_arguments_are_not_valid()
        {
            Action createAction = () => new NewListing(Guid.NewGuid(),
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                default);

            createAction.Should().Throw<ArgumentNullException>();
        }
    }
}
