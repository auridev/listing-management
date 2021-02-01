using Core.Domain.ValueObjects;
using Core.Domain.Offers;
using FluentAssertions;
using System;
using Xunit;
using Common.Helpers;
using Test.Helpers;
using LanguageExt;

namespace BusinessLine.Core.Domain.UnitTests.Offers
{
    public class ActiveOffer_should
    {
        private static readonly Owner _owner = TestValueObjectFactory.CreateOwner(Guid.NewGuid());
        private static readonly MonetaryValue _monetaryValue = TestValueObjectFactory.CreateMonetaryValue(2.4M, "UUU");
        private readonly ActiveOffer _sut = new ActiveOffer(Guid.NewGuid(), _owner, _monetaryValue, DateTimeOffset.UtcNow);

        [Fact]
        public void be_markable_as_seen()
        {
            // arrange
            SeenDate seenDate = TestValueObjectFactory.CreateSeenDate(DateTimeOffset.UtcNow);

            // act
            Either<Error, Unit> action = _sut.HasBeenSeen(seenDate);

            // assert
            action
                .Right(_ =>
                {
                    _sut.SeenDate.IsSome.Should().BeTrue();
                    _sut.SeenDate.Some(seenDate => seenDate.Value.Should().BeCloseTo(DateTimeOffset.UtcNow, 5_000));
                })
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void ignore_invalid_seendatet()
        {
            // act
            Either<Error, Unit> action = _sut.HasBeenSeen(null);

            // assert
            action
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Message.Should().Be("seenDate");
                    error.Should().BeOfType<Error.Invalid>();
                });
        }
    }
}
