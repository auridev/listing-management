using Common.Dates;
using Common.Helpers;
using Core.Application.Listings.Commands.ReceiveOffer.Factory;
using Core.Domain.Offers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.ReceiveOffer.Factory
{
    public class OfferFactory_should
    {
        private static Owner _owner = TestValueObjectFactory.CreateOwner(Guid.NewGuid());
        private static MonetaryValue _monetaryValue = TestValueObjectFactory.CreateMonetaryValue(2M, "AAA");

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
        public void return_EitherRight_with_ReceivedOffer_on_success()
        {
            // act
            Either<Error, ReceivedOffer> eitherReceivedOffer = _sut.Create(_owner, _monetaryValue);

            //assert
            eitherReceivedOffer
                .Right(offer => offer.Should().NotBeNull())
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        public static IEnumerable<object[]> InvalidArguments => new List<object[]>
        {
            new object[] { null, _monetaryValue, "owner" },
            new object[] { _owner, null, "monetaryValue" }
        };

        [Theory]
        [MemberData(nameof(InvalidArguments))]
        public void return_EitherLeft_with_proper_error_when_arguments_are_invalid(Owner owner, MonetaryValue monetaryValue, string errorMessage)
        {
            // act
            Either<Error, ReceivedOffer> eitherReceivedOffer = _sut.Create(owner, monetaryValue);

            //assert
            eitherReceivedOffer
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be(errorMessage);
                });
        }
    }
}
