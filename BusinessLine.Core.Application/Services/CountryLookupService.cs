using Core.Domain.Common;
using LanguageExt;

namespace Core.Application.Services
{
    public sealed class CountryLookupService : ICountryLookupService
    {
        private readonly Country[] _countries = new Country[]
            {
                Country.Create("Lithuania",
                    Alpha2Code.Create("LT"),
                    Alpha3Code.Create("LTU"),
                    Currency.Euro),

                Country.Create("Latvia",
                    Alpha2Code.Create("LV"),
                    Alpha3Code.Create("LVA"),
                    Currency.Euro),

                Country.Create("Estonia",
                    Alpha2Code.Create("EE"),
                    Alpha3Code.Create("EST"),
                    Currency.Euro),

                Country.Create("Poland",
                    Alpha2Code.Create("PL"),
                    Alpha3Code.Create("POL"),
                    Currency.Create(CurrencyCode.Create("PLN"),
                        "zł",
                        "PZloty")),

                Country.Create("Germany",
                    Alpha2Code.Create("DE"),
                    Alpha3Code.Create("DEU"),
                    Currency.Euro),

                Country.Create("France",
                    Alpha2Code.Create("FR"),
                    Alpha3Code.Create("FRA"),
                    Currency.Euro),

                Country.Create("United Kingdom",
                    Alpha2Code.Create("GB"),
                    Alpha3Code.Create("GBR"),
                    Currency.Create(CurrencyCode.Create("GBP"),
                        "£",
                        "Pound Sterling")),
            };



        public Option<Country> GetCountryByAlpha2Code(string alpha2Code)
        {
            var alpha2 = Alpha2Code.Create(alpha2Code);

            return _countries
                .Find(c => c.Alpha2Code == alpha2);
        }
    }
}
