using Core.Domain.ValueObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class GeographicLocation_should
    {
        [Fact]
        public void have_Latitude_property()
        {
            var geographicLocation = GeographicLocation.Create(20D, 30D);
            geographicLocation.Latitude.Should().Be(20D);
        }

        [Fact]
        public void have_Longitude_property()
        {
            var geographicLocation = GeographicLocation.Create(1D, 2D);
            geographicLocation.Longitude.Should().Be(2D);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void throw_an_exception_during_creation_if_value_is_not_valid(double latitude, double longitude)
        {
            Action create = () => GeographicLocation.Create(latitude, longitude);

            create.Should().Throw<ArgumentException>();
        }

        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { 86, 181 },
            new object[] { 85.05112879, 181.1 },
            new object[] { -86, -181  },
            new object[] { -85.05112879, -181.1  },
            new object[] { 1, -182 },
            new object[] { 90, 50 }
        };

        [Fact]
        public void be_treated_as_equal_using_Equals_method_if_Latitudes_and_Longitudes_match()
        {
            var first = GeographicLocation.Create(5.5D, 30.1D);
            var second = GeographicLocation.Create(5.5D, 30.1D);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Latitudes_and_Longitudes_match()
        {
            var first = GeographicLocation.Create(-2.89D, 23.76D);
            var second = GeographicLocation.Create(-2.89D, 23.76D);

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Theory]
        [InlineData(6.3D, 10D, 5.8D, 20D)]
        [InlineData(6.3D, 10D, 5.8D, 10D)]
        [InlineData(5.8D, 10D, 5.8D, 20D)]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_either_Latitudes_and_Longitudes_dont_match
            (
                double firstLatitude,
                double firstLongitude,
                double secondLatitude,
                double secondLongitude
            )
        {
            var first = GeographicLocation.Create(firstLatitude, firstLongitude);
            var second = GeographicLocation.Create(secondLatitude, secondLongitude);

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
