using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class GeographicLocation_should
    {
        [Fact]
        public void have_Latitude_property()
        {
            GeographicLocation
                .Create(20D, 30D)
                .Right(location => location.Latitude.Should().Be(20D))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_Longitude_property()
        {
            GeographicLocation
                .Create(1D, 2D)
                .Right(location => location.Longitude.Should().Be(2D))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void return_EiherLeft_with_proper_error_during_creation_if_arguments_are_not_valid(double latitude, double longitude)
        {
            Either<Error, GeographicLocation> eitherGeographicLocation = GeographicLocation.Create(latitude, longitude);


            eitherGeographicLocation.IsLeft.Should().BeTrue();
            eitherGeographicLocation
               .Right(_ => throw InvalidExecutionPath.Exception)
               .Left(error => error.Should().BeOfType<Error.Invalid>());
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
        public void be_treated_as_equal_using_generic_Equals_method_if_Latitudes_and_Longitudes_match()
        {
            var first = GeographicLocation.Create(5.5D, 30.1D);
            var second = GeographicLocation.Create(5.5D, 30.1D);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_Equals_method_if_Latitudes_and_Longitudes_match()
        {
            var first = (object)GeographicLocation.Create(5.5D, 30.1D);
            var second = (object)GeographicLocation.Create(5.5D, 30.1D);

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
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_either_Latitudes_and_Longitudes_dont_match(
            double firstLatitude,
            double firstLongitude,
            double secondLatitude,
            double secondLongitude)
        {
            var first = GeographicLocation.Create(firstLatitude, firstLongitude);
            var second = GeographicLocation.Create(secondLatitude, secondLongitude);

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
