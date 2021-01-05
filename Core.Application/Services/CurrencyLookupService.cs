using Core.Domain.ValueObjects;
using LanguageExt;

namespace Core.Application.Services
{
    public sealed class CurrencyLookupService : ICurrencyLookupService
    {
        private readonly Currency[] _currencies = new Currency[]
        {
            Currency.Euro,
            Currency.USDollar,
            Currency.Create(CurrencyCode.Create("PLN"), "zł", "PZloty"),
            Currency.Create(CurrencyCode.Create("GBP"), "£", "Pound Sterling")
        };

        public Option<Currency> GetByCode(string code)
        {
            var currencyCode = CurrencyCode.Create(code);

            return _currencies
                .Find(c => c.Code == currencyCode);
        }
    }
}
