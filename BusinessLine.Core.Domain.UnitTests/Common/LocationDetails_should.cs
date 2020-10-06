using BusinessLine.Core.Domain.Common;
using FluentAssertions;
using LanguageExt;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class LocationDetails_should
    {
        private readonly LocationDetails _sut;

        public LocationDetails_should()
        {
            _sut = LocationDetails.Create(
                Alpha2Code.Create("al"),
                State.Create("staaaat"),
                City.Create("polis"),
                PostCode.Create("aaa1"),
                Address.Create("some random place 12"));
        }

        [Fact]
        public void have_a_CountryCode_property()
        {
            _sut.CountryCode.Value.Should().Be("AL");
        }

        [Fact]
        public void have_a_State_property()
        {
            _sut.State.Name.Value.Should().Be("Staaaat");
        }

        [Fact]
        public void have_a_City_property()
        {
            _sut.City.Name.ToString().Should().Be("Polis");
        }

        [Fact]
        public void have_a_PostCode_property()
        {
            _sut.PostCode.Value.ToString().Should().Be("aaa1");
        }

        [Fact]
        public void have_an_Address_property()
        {
            _sut.Address.Value.ToString().Should().Be("some random place 12");
        }

        [Fact]
        public void not_require_State_property()
        {
            var details = LocationDetails.Create(
                Alpha2Code.Create("al"),
                State.CreateNone(),
                City.Create("polis"),
                PostCode.Create("aaa1"),
                Address.Create("some random place 12"));

            details.State.Should().BeOfType(typeof(NoState));
        }

        [Fact]
        public void be_treated_as_equal_using_Equals_method_if_all_values_match()
        {
            var first = LocationDetails.Create(
               Alpha2Code.Create("pl"),
                State.CreateNone(),
                City.Create("warshaw"),
                PostCode.Create("12"),
                Address.Create("ddddd"));
            var second = LocationDetails.Create(
                Alpha2Code.Create("pl"),
                State.CreateNone(),
                City.Create("warshaw"),
                PostCode.Create("12"),
                Address.Create("ddddd")
            );

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_all_values_match()
        {
            var first = LocationDetails.Create(
                Alpha2Code.Create("us"),
                State.Create("123"),
                City.Create("la"),
                PostCode.Create("la123"),
                Address.Create("under a bridge"));
            var second = LocationDetails.Create(
                Alpha2Code.Create("us"),
                State.Create("123"),
                City.Create("la"),
                PostCode.Create("la123"),
                Address.Create("under a bridge"));

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_any_value_doesnt_match()
        {
            var first = LocationDetails.Create(
                Alpha2Code.Create("us"),
                State.Create("123"),
                City.Create("la"),
                PostCode.Create("la123"),
                Address.Create("under a bridge"));
            var second = LocationDetails.Create(
                Alpha2Code.Create("us"),
                State.CreateNone(),
                City.Create("la"),
                PostCode.Create("la123"),
                Address.Create("under a bridge"));

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
