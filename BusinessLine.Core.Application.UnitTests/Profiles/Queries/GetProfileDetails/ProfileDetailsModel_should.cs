using Core.Application.Profiles.Queries.GetProfileDetails;
using FluentAssertions;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Profiles.Queries.GetProfileDetails
{
    public class ProfileDetailsModel_should
    {
        private readonly ProfileDetailsModel _sut;

        public ProfileDetailsModel_should()
        {
            _sut = new ProfileDetailsModel()
            {
                Id = "a",
                Email = "a",
                FirstName = "a",
                LastName = "a",
                Company = "a",
                Phone = "a",
                CountryCode = "a",
                State = "a",
                City = "a",
                PostCode = "a",
                Address = "a",
                Latitude = 1D,
                Longitude = 1D,
                DistanceUnit = "a",
                MassUnit = "a",
                CurrencyCode = "a",
                IsActive = true
            };
        }

        [Fact]
        public void have_an_Id_property()
        {
            _sut.Id.Should().NotBeEmpty();
        }

        [Fact]
        public void have_an_Email_property()
        {
            _sut.Email.Should().NotBeEmpty();
        }

        [Fact]
        public void have_a_FirstName_property()
        {
            _sut.FirstName.Should().NotBeEmpty();
        }

        [Fact]
        public void have_a_LastName_property()
        {
            _sut.LastName.Should().NotBeEmpty();
        }

        [Fact]
        public void have_a_Company_property()
        {
            _sut.Company.Should().NotBeEmpty();
        }

        [Fact]
        public void have_a_Phone_property()
        {
            _sut.Phone.Should().NotBeEmpty();
        }

        [Fact]
        public void have_a_CountryCode_property()
        {
            _sut.CountryCode.Should().NotBeEmpty();
        }

        [Fact]
        public void have_a_State_property()
        {
            _sut.State.Should().NotBeEmpty();
        }

        [Fact]
        public void have_a_City_property()
        {
            _sut.City.Should().NotBeEmpty();
        }

        [Fact]
        public void have_a_PostCode_property()
        {
            _sut.PostCode.Should().NotBeEmpty();
        }

        [Fact]
        public void have_an_Address_property()
        {
            _sut.Address.Should().NotBeEmpty();
        }

        [Fact]
        public void have_a_Latitude_property()
        {
            _sut.Latitude.Should().BePositive();
        }

        [Fact]
        public void have_a_Longitude_property()
        {
            _sut.Longitude.Should().BePositive();
        }

        [Fact]
        public void have_a_DistanceUnit_property()
        {
            _sut.DistanceUnit.Should().NotBeEmpty();
        }

        [Fact]
        public void have_a_MassUnit_property()
        {
            _sut.MassUnit.Should().NotBeEmpty();
        }

        [Fact]
        public void have_a_CurrencyCode_property()
        {
            _sut.CurrencyCode.Should().NotBeEmpty();
        }

        [Fact]
        public void have_an_IsActive_property()
        {
            _sut.IsActive.Should().Be(true || false);
        }
    }
}
