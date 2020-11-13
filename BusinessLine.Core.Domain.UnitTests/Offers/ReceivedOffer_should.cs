using Core.Domain.Common;
using Core.Domain.Offers;
using FluentAssertions;
using System;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Offers
{
    public class ReceivedOffer_should
    {
        private static readonly Owner _owner = Owner.Create(Guid.NewGuid());
        private static readonly MonetaryValue _monetaryValue = MonetaryValue.Create(2.4M, CurrencyCode.Create("UUU"));
        private static readonly SeenDate _seenDate = SeenDate.Create(DateTimeOffset.UtcNow);

        private readonly ReceivedOffer _sut;
        public ReceivedOffer_should()
        {
            _sut = new ReceivedOffer(Guid.NewGuid(), _owner, _monetaryValue, DateTimeOffset.UtcNow, _seenDate);
        }

        [Fact]
        public void have_SeenDate_property()
        {
            _sut.SeenDate.Some(seenDate => seenDate.Value.Should().BeCloseTo(DateTimeOffset.UtcNow, 5_000));
        }

        [Fact]
        public void be_markable_as_seen()
        {
            var offer = new ReceivedOffer(Guid.NewGuid(),
                _owner,
                _monetaryValue,
                DateTimeOffset.UtcNow,
                SeenDate.Create(DateTimeOffset.UtcNow));

            offer.HasBeenSeen(_seenDate);

            offer.SeenDate.Some(seenDate => seenDate.Value.Should().BeCloseTo(DateTimeOffset.UtcNow, 5_000));
        }
    }
}
