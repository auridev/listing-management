using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class ContactDetails_should
    {
        private readonly Either<Error, ContactDetails> _sut;

        public ContactDetails_should()
        {
            _sut = ContactDetails.Create("aaaa", "bbbb", "cccc", "+333 111 22222");
        }

        [Fact]
        public void have_PersonName_property()
        {
            _sut
               .Right(ct => ct.PersonName.FullName.Should().Be("Aaaa Bbbb"))
               .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_Company_property()
        {
            _sut
               .Right(ct => ct.Company.Some(name => name.ToString().Should().Be("cccc")))
               .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_Phone_property()
        {
            _sut
               .Right(ct => ct.Phone.Number.Should().Be("+333 111 22222"))
               .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void not_require_Company()
        {
            ContactDetails
                .Create("bob", "marley", string.Empty, "333-11-444-555-666")
                .Right(ct => ct.Company.IsNone.Should().BeTrue())
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void be_treated_as_equal_using_generic_Equals_method_if_all_values_match()
        {
            var first = ContactDetails.Create("luke", "skywalker", "Rebelion", "111-22-333-444-555");
            var second = ContactDetails.Create("luke", "skywalker", "Rebelion", "111-22-333-444-555");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_all_values_match()
        {
            var first = (object)ContactDetails.Create("luke", "skywalker", "Rebelion", "111-22-333-444-555");
            var second = (object)ContactDetails.Create("luke", "skywalker", "Rebelion", "111-22-333-444-555");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_all_values_match()
        {
            var first = ContactDetails.Create("han", "solo", "Rebelion", "999-11-222-111-111");
            var second = ContactDetails.Create("han", "solo", "Rebelion", "999-11-222-111-111");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_any_value_doesnt_match()
        {
            var first = ContactDetails.Create("han", "solo", "Empire", "999-11-222-111-111");
            var second = ContactDetails.Create("han", "solo", "Rebelion", "999-11-222-111-111");

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
