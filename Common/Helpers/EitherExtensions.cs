using LanguageExt;
using System;

namespace Common.Helpers
{
    public static class EitherExtensions
    {
        public static TRight ToUnsafeRight<TLeft, TRight>(this Either<TLeft, TRight> result)
        {
            return result
                .Right(right => right)
                .Left(_ => throw new InvalidOperationException());
        }
    }
}
