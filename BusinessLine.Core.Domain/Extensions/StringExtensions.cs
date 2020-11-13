using System;
using System.Globalization;
using System.Linq;

namespace Core.Domain.Extensions
{
    public static class StringExtensions
    {
        public static string CapitalizeWords(this string input)
        {
            return CultureInfo
                .CurrentCulture
                .TextInfo
                .ToTitleCase(input.ToLower());
        }
        public static string CapitalizeFirstLetter(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentNullException(nameof(input));

            if (input.Length > 1)
                return char.ToUpper(input[0]) + input.Substring(1);

            return input.ToUpper();
        }
    }
}
