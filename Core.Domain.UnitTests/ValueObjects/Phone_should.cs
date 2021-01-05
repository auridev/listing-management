using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System;
using System.Collections.Generic;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class Phone_should
    {
        [Fact]
        public void have_a_Number_property()
        {
            Either<Error, Phone> eitherPhone = Phone.Create("+333 111 22222");

            eitherPhone
                .Right(phone => phone.Number.Should().Be("+333 111 22222"))
                .Left(_ => throw new InvalidOperationException());
        }

        [Theory]
        [MemberData(nameof(InvalidNumbers))]
        public void return_Eiher_in_Left_state_with_proper_error_during_creation_if_argument_is_not_valid(string value)
        {
            Either<Error, Phone> eitherPhone = Phone.Create(value);

            eitherPhone.IsLeft.Should().BeTrue();
            eitherPhone
                .Right(_ => throw new InvalidOperationException())
                .Left(error => error.Should().BeOfType<Error.Invalid>());
        }

        public static IEnumerable<object[]> InvalidNumbers => new List<object[]>
        {
            new object[] { null },
            new object[] { string.Empty },
            new object[] { "" },
            new object[] { default },
            new object[] { "aaadasdasdasdasd" },
            new object[] { "1" },
            new object[] { "+ 1" },
            new object[] { "113.203.300" },
            new object[] { "+44.504.215.278" },
            new object[] { "+370(123) 456-78-90-" }
        };

        [Theory]
        [MemberData(nameof(ValidNumbers))]
        public void set_Number_property_if_number_format_is_valid(string number)
        {
            Either<Error, Phone> eitherPhone = Phone.Create(number);

            eitherPhone
               .Right(phone => phone.Number.Should().Be(number))
               .Left(_ => throw new InvalidOperationException());
        }

        public static IEnumerable<object[]> ValidNumbers => new List<object[]>
        {
            new object[] { "+333 (11) 444 555 666" },
            new object[] { "+333 (11) 444-555-666"},
            new object[] { "+333(11)444555666" },
            new object[] { "+333 11 444555666" },
            new object[] { "+33311444555666" },
            new object[] { "33311444555666" },
            new object[] { "333-11-444-555-666" }
        };

        [Fact]
        public void not_have_any_leading_or_trailing_whitespaces_in_Number_property()
        {
            Either<Error, Phone> eitherPhone = Phone.Create("   +333 (11) 444 555 666      ");

            eitherPhone
               .Right(phone => phone.Number.Should().Be("+333 (11) 444 555 666"))
               .Left(_ => throw new InvalidOperationException());
        }

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_Numbers_match()
        {
            var first = Phone.Create("999 333 11111");
            var second = Phone.Create("999 333 11111");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_Numbers_match()
        {
            var first = (object)Phone.Create("999 333 11111");
            var second = (object)Phone.Create("999 333 11111");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Numbers_match()
        {
            var first = Phone.Create("111 222 33333");
            var second = Phone.Create("111 222 33333");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Numbers_dont_match()
        {
            var first = Phone.Create("111 111 11111");
            var second = Phone.Create("222 222 22222");

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
