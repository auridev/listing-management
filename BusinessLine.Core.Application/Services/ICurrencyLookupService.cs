using BusinessLine.Core.Domain.Common;
using LanguageExt;

namespace BusinessLine.Core.Application.Services
{
    public interface ICurrencyLookupService
    {
        Option<Currency> GetByCode(string code);
    }
}