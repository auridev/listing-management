using BusinessLine.Core.Domain.Common;
using FluentAssertions;
using Xunit;


namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class Company_should
    {
        [Fact]
        public void have_a_Name_property()
        {
            var company = Company.Create("some name");
            company.Name.Value.Should().Be("some name");
        }

        [Fact]
        public void be_NoCompany_if_name_is_not_valid()
        {
            var company = Company.Create(string.Empty);
            company.Should().BeOfType(typeof(NoCompany));
        }

        [Fact]
        public void be_treated_as_equal_using_Equals_method_if_Names_match()
        {
            var first = Company.Create("asd");
            var second = Company.Create("asd");

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Names_match()
        {
            var first = Company.Create("qwe");
            var second = Company.Create("qwe");

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Names_dont_match()
        {
            var first = Company.Create("rt");
            var second = Company.Create("yu");

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }

        [Fact]
        public void have_NoCompany_with_default_Name_value()
        {
            var noCompany = Company.Create(string.Empty);

            noCompany.Name.Should().Be(TrimmedString.None);
        }

        [Fact]
        public void have_CreateNone_for_explicit_NoCompany_creation()
        {
            var company = Company.CreateNone();

            company.Should().BeOfType(typeof(NoCompany));
        }
    }
}
