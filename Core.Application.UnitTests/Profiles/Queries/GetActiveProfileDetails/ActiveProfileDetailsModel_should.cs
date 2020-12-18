using Core.Application.Profiles.Queries.GetActiveProfileDetails;
using FluentAssertions;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Profiles.Queries.GetProfileDetails
{
    public class ActiveProfileDetailsModel_should
    {
        private readonly ActiveProfileDetailsModel _sut;
        private readonly DateTimeOffset _introductionSeenDate = DateTimeOffset.UtcNow;
        private readonly DateTimeOffset _createdDate = DateTimeOffset.UtcNow;

        public ActiveProfileDetailsModel_should()
        {
            _sut = new ActiveProfileDetailsModel()
            {
                Id = new Guid("d3a96843-d41d-4aca-b5f0-c683fbe96e1e"),
                UserId = new Guid("1b2e7e5a-0429-4c90-8092-b5a1165dbc2e"),
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
                IntroductionSeenOn = _introductionSeenDate,
                CreatedDate = _createdDate
            };
        }

        [Fact]
        public void have_an_Id_property()
        {
            _sut.Id.Should().NotBeEmpty();
        }

        [Fact]
        public void have_UserId_property()
        {
            _sut.UserId.Should().NotBeEmpty();
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
        public void have_IntroductionSeenOn_property()
        {
            _sut.IntroductionSeenOn.Should().BeCloseTo(_introductionSeenDate);
        }

        [Fact]
        public void have_CreatedDate_property()
        {
            _sut.CreatedDate.Should().BeCloseTo(_createdDate);
        }
    }
}
