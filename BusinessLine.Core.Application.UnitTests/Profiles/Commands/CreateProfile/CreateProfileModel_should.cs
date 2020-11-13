using Core.Application.Profiles.Commands.CreateProfile;
using FluentAssertions;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Profiles.Commands.CreateProfile
{
    public class CreateProfileModel_should
    {
        private readonly CreateProfileModel _sut;

        public CreateProfileModel_should()
        {
            _sut = new CreateProfileModel()
            {
                Email = "my@email.com",
                FirstName = "aaaaaa",
                LastName = "bbbbb",
                Company = "google",
                Phone = "+333 111 22222",
                CountryCode = "d",
                State = "sss",
                City = "utena",
                PostCode = "pcode",
                Address = "my place 1",
                Latitude = 23D,
                Longitude = 100D,
                DistanceUnit = "kilometer",
                MassUnit = "pound",
                CurrencyCode = "eur"
            };
        }

        [Fact]
        public void have_an_Email_property()
        {
            _sut.Email.Should().Be("my@email.com");
        }

        [Fact]
        public void have_a_FirstName_property()
        {
            _sut.FirstName.Should().Be("aaaaaa");
        }

        [Fact]
        public void have_a_LastName_property()
        {
            _sut.LastName.Should().Be("bbbbb");
        }

        [Fact]
        public void have_a_Company_property()
        {
            _sut.Company.Should().Be("google");
        }

        [Fact]
        public void have_a_Phone_property()
        {
            _sut.Phone.Should().Be("+333 111 22222");
        }

        [Fact]
        public void have_a_CountryCode_property()
        {
            _sut.CountryCode.Should().Be("d");
        }

        [Fact]
        public void have_a_State_property()
        {
            _sut.State.Should().Be("sss");
        }

        [Fact]
        public void have_a_City_property()
        {
            _sut.City.Should().Be("utena");
        }

        [Fact]
        public void have_a_PostCode_property()
        {
            _sut.PostCode.Should().Be("pcode");
        }

        [Fact]
        public void have_an_Address_property()
        {
            _sut.Address.Should().Be("my place 1");
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
            _sut.DistanceUnit.Should().Be("kilometer");
        }

        [Fact]
        public void have_a_MassUnit_property()
        {
            _sut.MassUnit.Should().Be("pound");
        }

        [Fact]
        public void have_a_CurrencyCode_property()
        {
            _sut.CurrencyCode.Should().Be("eur");
        }
    }
}
