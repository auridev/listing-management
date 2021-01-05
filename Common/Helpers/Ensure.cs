using System;

namespace Common.Helpers
{
    public static class Ensure
    {
        public static T NotNull<T>(T parameterValue, string parameterName)
        {
            if (parameterValue is null) 
                throw new ArgumentNullException(parameterName);

            return parameterValue;
        }
    }
}

