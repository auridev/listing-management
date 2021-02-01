using Common.ApplicationSettings;
using FluentAssertions;
using Xunit;

namespace Common.UnitTests.ApplicationSettings
{
    public class ConnectionStrings_should
    {
        [Fact]
        public void have_BusinessLine_property()
        {
            var sut = new ConnectionStrings();

            sut.Listings.Should().BeNull();
        }
    }
}
