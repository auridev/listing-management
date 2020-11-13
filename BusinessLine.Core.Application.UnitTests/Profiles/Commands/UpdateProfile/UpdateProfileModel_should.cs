using Core.Application.Profiles.Commands.UpdateProfile;
using FluentAssertions;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Profiles.Commands.UpdateProfile
{
    public class UpdateProfileModel_should
    {
        private readonly UpdateProfileModel _sut;

        public UpdateProfileModel_should()
        {
            _sut = new UpdateProfileModel()
            {
                FirstName = "name",
                LastName = "last",
                Company = "air",
                Phone = "+111 111 11111",
                CountryCode = "add",
                State = "state",
                City = "wilno",
                PostCode = "code",
                Address = "pietu 14",
                Latitude = 23D,
                Longitude = 100D,
                DistanceUnit = "m",
                MassUnit = "kg",
                CurrencyCode = "eur"
            };
        }

        [Fact]
        public void have_a_FirstName_property()
        {
            _sut.FirstName.Should().Be("name");
        }

        [Fact]
        public void have_a_LastName_property()
        {
            _sut.LastName.Should().Be("last");
        }

        [Fact]
        public void have_a_Company_property()
        {
            _sut.Company.Should().Be("air");
        }

        [Fact]
        public void have_a_Phone_property()
        {
            _sut.Phone.Should().Be("+111 111 11111");
        }

        [Fact]
        public void have_a_CountryCode_property()
        {
            _sut.CountryCode.Should().Be("add");
        }

        [Fact]
        public void have_a_State_property()
        {
            _sut.State.Should().Be("state");
        }

        [Fact]
        public void have_a_City_property()
        {
            _sut.City.Should().Be("wilno");
        }

        [Fact]
        public void have_a_PostCode_property()
        {
            _sut.PostCode.Should().Be("code");
        }

        [Fact]
        public void have_an_Address_property()
        {
            _sut.Address.Should().Be("pietu 14");
        }

        [Fact]
        public void have_a_Latitude_property()
        {
            _sut.Latitude.Should().Be(23);
        }

        [Fact]
        public void have_a_Longitude_property()
        {
            _sut.Longitude.Should().Be(100);
        }

        [Fact]
        public void have_a_DistanceUnit_property()
        {
            _sut.DistanceUnit.Should().Be("m");
        }

        [Fact]
        public void have_a_MassUnit_property()
        {
            _sut.MassUnit.Should().Be("kg");
        }

        [Fact]
        public void have_a_CurrencyCode_property()
        {
            _sut.CurrencyCode.Should().Be("eur");
        }
    }
}
