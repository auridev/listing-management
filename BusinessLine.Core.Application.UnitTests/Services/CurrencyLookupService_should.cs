using Core.Application.Services;
using Core.Domain.Common;
using FluentAssertions;
using LanguageExt;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Services
{
    public class CurrencyLookupService_should
    {
        private readonly CurrencyLookupService _sut;

        public CurrencyLookupService_should()
        {
            _sut = new CurrencyLookupService();
        }

        [Fact]
        public void return_a_maching_currency_by_code()
        {
            Option<Currency> possibleCurrency = _sut.GetByCode("usd");

            possibleCurrency.IsSome.Should().BeTrue();
        }

        [Fact]
        public void return_a_none_option_if_no_currency()
        {
            Option<Currency> possibleCurrency = _sut.GetByCode("d34");

            possibleCurrency.IsNone.Should().BeTrue();
        }
    }
}
