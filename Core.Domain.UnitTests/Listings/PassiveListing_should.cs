using Common.Helpers;
using Core.Domain.Listings;
using FluentAssertions;
using LanguageExt;
using System;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Listings
{
    public class PassiveListing_should : Listing_should
    {
        private readonly PassiveListing _sut = new PassiveListing(
            Guid.NewGuid(),
            _owner,
            _listingDetails,
            _contactDetails,
            _locationDetails,
            _geographicLocation,
            _createdDate,
            TestValueObjectFactory.CreateTrimmedString("something bad"));

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
                TestValueObjectFactory.CreateTrimmedString("aaaaa"));

            createAction.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void thrown_an_exception_during_creation_if_reason_is_null()
        {
            Action createAction = () => new PassiveListing(Guid.NewGuid(),
                _owner,
                _listingDetails,
                _contactDetails,
                _locationDetails,
                _geographicLocation,
                _createdDate,
                null);

            createAction.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void have_a_Reason_property()
        {
            _sut.Reason.ToString().Should().Be("something bad");
        }

        [Fact]
        public void be_reactivatable()
        {
            // act
            Either<Error, ActiveListing> action = _sut.Reactivate(DateTimeOffset.UtcNow.AddDays(90));

            // assert
            action
                .Right(activeListing =>
                {
                    activeListing.Should().NotBeNull();
                    activeListing.ExpirationDate.Should().BeCloseTo(DateTimeOffset.UtcNow.AddDays(90));
                })
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void reject_to_be_reactivated_if_expiration_date_is_not_valid()
        {
            Either<Error, ActiveListing> action = _sut.Reactivate(default);

            action
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error => error.Should().BeOfType<Error.Invalid>());
        }
    }
}
