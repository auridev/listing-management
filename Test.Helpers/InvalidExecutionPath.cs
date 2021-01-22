using System;

namespace Test.Helpers
{
    public class InvalidExecutionPath
    {
        public readonly static InvalidOperationException Exception = new InvalidOperationException("shouldn't reach this code");
    }
}
