﻿using Core.Application.Listings.Commands.ReceiveOffer.Factory;
using Core.Domain.Common;
using Core.Domain.Offers;
using Common.Dates;
using FluentAssertions;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.ReceiveOffer.Factory
{
    public class OfferFactory_should
    {
        private readonly OfferFactory _sut;
        private readonly AutoMocker _mocker;
        public OfferFactory_should()
        {
            _mocker = new AutoMocker();

            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetCurrentUtcDateTime())
                .Returns(DateTimeOffset.UtcNow);

            _sut = _mocker.CreateInstance<OfferFactory>();
        }

        [Fact]
        public void create_offers()
        {
            var owner = Owner.Create(Guid.NewGuid());
            var monetaryValue = MonetaryValue.Create(2M, CurrencyCode.Create("AAA"));

            ReceivedOffer offer = _sut.Create(owner, monetaryValue);

            offer.Should().NotBeNull();
            offer.Id.Should().NotBeEmpty();
            offer.SeenDate.IsNone.Should().BeTrue();
        }
    }
}