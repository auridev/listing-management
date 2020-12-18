using Core.Domain.Common;
using Core.Domain.Listings;
using Core.Domain.Offers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Listings
{
    public class ClosedListing_should : Listing_should
    {
        private static readonly AcceptedOffer _acceptedOffer = new AcceptedOffer(Guid.NewGuid(),
                Owner.Create(Guid.NewGuid()),
                MonetaryValue.Create(12.5M, CurrencyCode.Create("eur")),
                DateTimeOffset.UtcNow);

        private static readonly List<RejectedOffer> _rejectedOffers = new List<RejectedOffer>();

        private readonly ClosedListing _sut;

        public ClosedListing_should()
        {
            _sut = new ClosedListing(Guid.NewGuid(),
               _owner,
               _listingDetails,
               _contactDetails,
               _locationDetails,
               _geographicLocation,
               _createdDate,
               DateTimeOffset.UtcNow.AddDays(-1),
               _acceptedOffer,
               _rejectedOffers);
        }

        [Fact]
        public void have_a_ClosedOn_property()
        {
            _sut.ClosedOn.Should().BeCloseTo(DateTimeOffset.UtcNow.AddDays(-1));
        }

        public static IEnumerable<object[]> InvalidArgumentsForClosedListing => new List<object[]>
        {
            new object[] { default, _acceptedOffer, _rejectedOffers  },
            new object[] { DateTimeOffset.Now, null, _rejectedOffers },
            new object[] { DateTimeOffset.Now, _acceptedOffer, null }
        };

        [Theory]
        [MemberData(nameof(InvalidArgumentsForClosedListing))]
        public void thrown_an_exception_during_creation_if_some_arguments_are_not_valid(DateTimeOffset closedOn, AcceptedOffer acceptedOffer, List<RejectedOffer> rejectedOffers)
        {
            Action createAction = () => new ClosedListing(Guid.NewGuid(), 
                _owner, 
                _listingDetails, 
                _contactDetails,
                _locationDetails, 
                _geographicLocation,
                _createdDate,
                closedOn,
                acceptedOffer,
                rejectedOffers);

            createAction.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void have_an_AcceptedOffer_property()
        {
            _sut.AcceptedOffer.Should().NotBeNull();
        }

        [Fact]
        public void have_a_RejectedOffers_property()
        {
            _sut.RejectedOffers.Should().NotBeNull();
        }
    }
}
