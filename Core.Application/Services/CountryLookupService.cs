using Common.Helpers;
using Core.Domain.ValueObjects;
using LanguageExt;

namespace Core.Application.Services
{
    public sealed class CountryLookupService : ICountryLookupService
    {
        private readonly Country[] _countries = new Country[]
            {
                Country.Create("Lithuania",
                    Alpha2Code.Create("LT").ToUnsafeRight(),
                    Alpha3Code.Create("LTU").ToUnsafeRight(),
                    Currency.Euro),

                Country.Create("Latvia",
                    Alpha2Code.Create("LV").ToUnsafeRight(),
                    Alpha3Code.Create("LVA").ToUnsafeRight(),
                    Currency.Euro),

                Country.Create("Estonia",
                    Alpha2Code.Create("EE").ToUnsafeRight(),
                    Alpha3Code.Create("EST").ToUnsafeRight(),
                    Currency.Euro),

                Country.Create("Poland",
                    Alpha2Code.Create("PL").ToUnsafeRight(),
                    Alpha3Code.Create("POL").ToUnsafeRight(),
                    Currency.Create(CurrencyCode.Create("PLN"),
                        "zł",
                        "PZloty")),

                Country.Create("Germany",
                    Alpha2Code.Create("DE").ToUnsafeRight(),
                    Alpha3Code.Create("DEU").ToUnsafeRight(),
                    Currency.Euro),

                Country.Create("France",
                    Alpha2Code.Create("FR").ToUnsafeRight(),
                    Alpha3Code.Create("FRA").ToUnsafeRight(),
                    Currency.Euro),

                Country.Create("United Kingdom",
                    Alpha2Code.Create("GB").ToUnsafeRight(),
                    Alpha3Code.Create("GBR").ToUnsafeRight(),
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
