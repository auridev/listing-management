using LanguageExt;
using static LanguageExt.Prelude;

namespace Common.Helpers
{
    public static class Result
    {
        public static Either<Error, TValue> Success<TValue>(TValue value)
            => Right<Error, TValue>(value);

        public static Either<Error, TValue> Invalid<TValue>(string message)
            => Left<Error, TValue>(new Error.Invalid(message));

        public static Either<Error, TValue> NotFound<TValue>(string message)
            => Left<Error, TValue>(new Error.NotFound(message));

        public static Either<Error, TValue> Unauthorized<TValue>(string message)
            => Left<Error, TValue>(new Error.Unauthorized(message));
    }
}
