using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Listings;
using FluentAssertions;
using System;
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
        public void have_a_CreatedDate_property()
        {
            _sut.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
        }

        [Fact]
        public void be_deactivatble()
        {
            // act
            PassiveListing passiveListing = _sut.Deactivate(TrimmedString.Create("somethings not right"), DateTimeOffset.UtcNow);

            // assert
            passiveListing.Should().NotBeNull();
            passiveListing.DeactivationDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
            passiveListing.Reason.Value.Should().Be("somethings not right");
        }

        [Fact]
        public void be_activatable()
        {
            // act
            DateTimeOffset expirationDate = DateTimeOffset.Now.AddDays(1);
            ActiveListing activeListing = _sut.Activate(expirationDate);

            // assert
            activeListing.Should().NotBeNull();
            activeListing.ExpirationDate.Should().BeCloseTo(DateTimeOffset.UtcNow.AddDays(1));
        }

        [Fact]
        public void be_markable_as_suspicious()
        {
            SuspiciousListing suspiciousListing = 
                _sut.MarkAsSuspicious(DateTimeOffset.UtcNow, TrimmedString.Create("for test"));

            suspiciousListing.Should().NotBeNull();
            suspiciousListing.MarkedAsSuspiciousAt.Should().BeCloseTo(DateTimeOffset.UtcNow);
            suspiciousListing.Reason.Value.Should().Be("for test");
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
