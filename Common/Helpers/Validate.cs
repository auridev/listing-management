using LanguageExt;
using static LanguageExt.Prelude;

namespace Common.Helpers
{
    public static class Validate
    {
        public static Validation<Error, string> IsNotNullOrWhiteSpace(string input) =>
            (!string.IsNullOrWhiteSpace(input))
                ? Success<Error, string>(input)
                : Fail<Error, string>(new Error.Invalid("invalid stirng"));
    }
}
