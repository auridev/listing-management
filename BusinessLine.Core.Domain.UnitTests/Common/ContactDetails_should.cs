using BusinessLine.Core.Domain.Common;
using FluentAssertions;
using LanguageExt;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class ContactDetails_should
    {
        private readonly ContactDetails _sut;

        public ContactDetails_should()
        {
            _sut = ContactDetails.Create(
                PersonName.Create("aaaa", "bbbb"),
                Company.Create("cccc"),
                Phone.Create("+333 111 22222"));
        }

        [Fact]
        public void have_PersonName_property()
        {
            _sut.PersonName.FullName.Should().Be("Aaaa Bbbb");
        }

        [Fact]
        public void have_Company_property()
        {
            _sut.Company.Name.ToString().Should().Be("cccc");
        }

        [Fact]
        public void have_Phone_property()
        {
            _sut.Phone.Number.Should().Be("+333 111 22222");
        }

        [Fact]
        public void not_require_Company()
        {
            var contactDetails = ContactDetails.Create
            (
                PersonName.Create("bob", "marley"),
                Company.CreateNone(),
                Phone.Create("333-11-444-555-666")
            );

            contactDetails.Company.Should().BeOfType(typeof(NoCompany));
            contactDetails.Company.Name.Should().Be(TrimmedString.None);
        }

        [Fact]
        public void be_treated_as_equal_using_Equals_method_if_all_values_match()
        {
            var first = ContactDetails.Create
            (
                PersonName.Create("luke", "skywalker"),
                Company.Create("Rebelion"),
                Phone.Create("111-22-333-444-555")
            );
            var second = ContactDetails.Create
            (
                PersonName.Create("luke", "skywalker"),
                Company.Create("Rebelion"),
                Phone.Create("111-22-333-444-555")
            );

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_all_values_match()
        {
            var first = ContactDetails.Create
            (
                PersonName.Create("han", "solo"),
                Company.CreateNone(),
                Phone.Create("999-11-222-111-111")
            );
            var second = ContactDetails.Create
            (
                PersonName.Create("han", "solo"),
                Company.CreateNone(),
                Phone.Create("999-11-222-111-111")
            );

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_any_value_doesnt_match()
        {
            var first = ContactDetails.Create
            (
                PersonName.Create("han", "solo"),
                Company.CreateNone(),
                Phone.Create("999-11-222-111-111")
            );
            var second = ContactDetails.Create
            (
                PersonName.Create("han", "solo"),
                Company.Create("Rebelion"),
                Phone.Create("999-11-222-111-111")
            );

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
