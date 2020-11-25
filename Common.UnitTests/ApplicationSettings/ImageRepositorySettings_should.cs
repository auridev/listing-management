using Common.ApplicationSettings;
using FluentAssertions;
using Xunit;

namespace Common.UnitTests.ApplicationSettings
{
    public class ImageRepositorySettings_should
    {
        [Fact]
        public void have_Location_property()
        {
            var sut = new ImageRepositorySettings();

            sut.Location.Should().BeNull();
        }
    }
}
