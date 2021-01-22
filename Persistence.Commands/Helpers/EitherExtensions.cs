using LanguageExt;
using System;

namespace Persistence.Commands.Helpers
{
    internal static class EitherExtensions
    {
        public static TRight ToUnsafeRight<TLeft, TRight>(this Either<TLeft, TRight> result)
        {
            return result
                .Right(right => right)
                .Left(_ => throw new InvalidOperationException());
        }
    }
}
