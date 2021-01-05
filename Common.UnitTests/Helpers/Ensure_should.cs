using Common.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Common.UnitTests.Helpers
{
    public class Ensure_should
    {
        [Theory]
        [MemberData(nameof(Arguments))]
        public void throw_an_exception_if_argument_is_null(string argument)
        {
            Action createAction = () => Ensure.NotNull<string>(argument, argument);

            createAction.Should().Throw<ArgumentException>();
        }

        public static IEnumerable<object[]> Arguments => new List<object[]>
        {
            new object[] { null },
            new object[] { default }
        };
    }
}
