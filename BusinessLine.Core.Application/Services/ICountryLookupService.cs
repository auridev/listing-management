using Core.Domain.Common;
using LanguageExt;

namespace Core.Application.Services
{
    public interface ICountryLookupService
    {
        Option<Country> GetCountryByAlpha2Code(string alpha2Code);
    }
}