using Common.Helpers;
using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System;
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
                _createdDate);
        }

        [Fact]
        public void be_markable_as_passive()
        {
            // arrange
            TrimmedString reason = TestValueObjectFactory.CreateTrimmedString("somethings not right");

            // act
            Either<Error, PassiveListing> deactivateAction = _sut.MarkAsPassive(reason);

            // assert
            deactivateAction
                .Right(passiveListing =>
                {
                    passiveListing.Should().NotBeNull();
                    passiveListing.Reason.Value.Should().Be("somethings not right");
                })
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void reject_to_be_markable_as_passive_if_reason_is_null()
        {
            Either<Error, PassiveListing> action = _sut.MarkAsPassive(null);

            action
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error => error.Should().BeOfType<Error.Invalid>());
        }

        [Fact]
        public void be_markable_as_active()
        {
            // arrange
            DateTimeOffset expirationDate = DateTimeOffset.Now.AddDays(1);

            // act
            Either<Error, ActiveListing> activeAction = _sut.MarkAsActive(expirationDate);

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
        public void reject_to_be_markable_as_active_if_expiration_date_is_not_valid()
        {
            Either<Error, ActiveListing> action = _sut.MarkAsActive(default);

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
            Either<Error, SuspiciousListing> action = _sut.MarkAsSuspicious(reason);

            action
                .Right(suspiciousListing =>
                {
                    suspiciousListing.Should().NotBeNull();
                    suspiciousListing.Reason.Value.Should().Be("for test");
                })
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void reject_to_be_marked_as_suspicious_if_reason_is_null()
        {
            Either<Error, SuspiciousListing> action = _sut.MarkAsSuspicious(null);

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
