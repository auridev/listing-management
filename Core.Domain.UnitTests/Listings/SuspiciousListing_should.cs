using Common.Helpers;
using Core.Domain.Listings;
using FluentAssertions;
using LanguageExt;
using System;
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
            TestValueObjectFactory.CreateTrimmedString("robot didnt understand description"));

        [Fact]
        public void have_a_Reason_property()
        {
            _sut.Reason.ToString().Should().Be("robot didnt understand description");
        }

        [Fact]
        public void be_deactivatable()
        {
            // act
            Either<Error, PassiveListing> action = _sut.Deactivate(TestValueObjectFactory.CreateTrimmedString("wrong number"));

            // assert
            action
                .Right(passiveListing =>
                {
                    passiveListing.Should().NotBeNull();
                    passiveListing.Reason.Value.Should().Be("wrong number");
                })
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void reject_to_be_deactivated_if_reason_is_null()
        {
            Either<Error, PassiveListing> action = _sut.Deactivate(null);

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

        [Fact]
        public void thrown_an_exception_during_creation_if_some_arguments_are_not_valid()
        {
            Action createAction = () => new SuspiciousListing(Guid.NewGuid(),
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _createdDate,
                null);

            createAction.Should().Throw<ArgumentNullException>();
        }
    }
}
