using Core.Domain.Common;
using LanguageExt;

namespace Core.Application.Services
{
    public interface ICurrencyLookupService
    {
        Option<Currency> GetByCode(string code);
    }
}