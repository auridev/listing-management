using BusinessLine.Core.Domain.Common;
using LanguageExt;

namespace BusinessLine.Core.Application.Services
{
    public interface ICountryLookupService
    {
        Option<Country> GetCountryByAlpha2Code(string alpha2Code);
    }
}