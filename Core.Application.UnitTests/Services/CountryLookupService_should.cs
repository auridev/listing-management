using Core.Application.Services;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Services
{
    public class CountryLookupService_should
    {
        private readonly CountryLookupService _sut;

        public CountryLookupService_should()
        {
            _sut = new CountryLookupService();
        }

        [Fact]
        public void return_a_maching_coutry_by_alpha2_code_string()
        {
            Option<Country> possibleCountry = _sut.GetCountryByAlpha2Code("lt");

            possibleCountry.IsSome.Should().BeTrue();
            possibleCountry.IfSome(c => c.Name.Should().Be("Lithuania"));
        }

        [Fact]
        public void return_a_none_option_if_no_coutry_matches()
        {
            Option<Country> possibleCountry = _sut.GetCountryByAlpha2Code("22");

            possibleCountry.IsNone.Should().BeTrue();
        }
    }
}
