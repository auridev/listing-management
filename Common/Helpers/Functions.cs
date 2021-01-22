using LanguageExt;
using System;
using System.Globalization;
using static Common.Helpers.Result;
using static LanguageExt.Prelude;

namespace Common.Helpers
{
    public static class Functions
    {
        public static Either<Error, string> EnsureNonEmpty(string input)
            =>
                (!string.IsNullOrWhiteSpace(input))
                    ? Right(input)
                    : Left(Invalid<string>("value cannot be empty"));

        public static Either<Error, string> Trim(Either<Error, string> input)
            =>
                input.Map(value => value.Trim());

        public static Either<Error, string> ConvertToUpper(Either<Error, string> input)
            =>
                input.Map(value => value.ToUpper());

        public static Either<Error, string> ConvertToLower(Either<Error, string> input)
             =>
                 input.Map(value => value.ToLower());

        public static Either<Error, string> EnsureRequiredLength(string input, int length)
            =>
                input != null && input.Length == length
                    ? Right(input)
                    : Left(Invalid<string>($"value needs to be {length} long"));

        public static Either<Error, string> EnsureMinLength(string input, int minLength)
            =>
                input != null && input.Length >= minLength
                    ? Right(input)
                    : Left(Invalid<string>($"value needs to be longer than {minLength}"));

        public static Either<Error, string> CapitalizeAllWords(Either<Error, string> input)
            =>
                input.Map(input => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower()));

        public static Either<Error, string> CapitalizeFirstLetter(Either<Error, string> input)
            =>
                input.Map(input =>
                {
                    if (input.Length > 1)
                        return char.ToUpper(input[0]) + input.Substring(1);

                    return input.ToUpper();
                });

        public static Either<Error, DateTimeOffset> EnsureNonDefault(DateTimeOffset dateTimeOffset)
            =>
                (dateTimeOffset != default)
                    ? Right(dateTimeOffset)
                    : Left(Invalid<DateTimeOffset>("invalid dateTimeOffset"));

        public static Either<Error, Guid> EnsureNonDefault(Guid value)
            =>
                (value != default)
                    ? Right(value)
                    : Left(Invalid<Guid>("invalid guid"));

        public static Either<Error, T> EnsureNotNull<T>(T value)
            =>
                (value != null)
                    ? Right(value)
                    : Left(Invalid<T>("cannot be null"));
    }
}
