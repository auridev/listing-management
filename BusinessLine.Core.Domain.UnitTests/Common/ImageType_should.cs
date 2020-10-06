using BusinessLine.Core.Domain.Common;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class ImageType_should
    {
        [Fact]
        public void have_Value_property()
        {
            var imageType = ImageType.JPEG;

            imageType.Value.Should().Be("jpeg");
        }

        [Fact]
        public void have_Id_property()
        {
            var imageType = ImageType.Unknown;

            imageType.Id.Should().Be(0);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void have_predefined_options(ImageType imageType, int expectedId, string expectedValue)
        {
            imageType.Id.Should().Be(expectedId);
            imageType.Value.Should().Be(expectedValue);
        }

        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { ImageType.Unknown, 0, "unknown" },
            new object[] { ImageType.JPEG, 10, "jpeg" },
            new object[] { ImageType.PNG, 20, "png" },
            new object[] { ImageType.GIF, 30, "gif" },
            new object[] { ImageType.BMP, 40, "bmp" }
        };

        [Fact]
        public void be_treated_as_equal_using_Equals_method_if_predefined_values_match()
        {
            var first = ImageType.Unknown;
            var second = ImageType.Unknown;

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_predefined_values_match()
        {
            var first = ImageType.JPEG;
            var second = ImageType.JPEG;

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_predefined_values_dont_match()
        {
            var first = ImageType.GIF;
            var second = ImageType.BMP;

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }

        [Fact]
        public void have_ByValue_method() 
        {
            ImageType imageType = ImageType.ByValue("jpeg");

            imageType.Should().NotBeNull();
        }

        [Fact]
        public void create_correct_image_type_by_value()
        {
            ImageType imageType = ImageType.ByValue("jpeg");

            imageType.Should().Be(ImageType.JPEG);
        }

        [Fact]
        public void create_unknown_for_unknown_values()
        {
            ImageType imageType = ImageType.ByValue("aaa");

            imageType.Should().Be(ImageType.Unknown);
        }

        [Fact]
        public void create_unknown_for_invalid_values()
        {
            ImageType imageType = ImageType.ByValue(null);

            imageType.Should().Be(ImageType.Unknown);
        }
    }
}
