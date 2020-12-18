using Core.Application.Profiles.Queries.GetPassiveProfileDetails;
using FluentAssertions;
using System;
using Xunit;

namespace Core.Application.UnitTests.Profiles.Queries.GetPassiveProfileDetails
{
    public class PassiveProfileDetailsModel_should
    {
        private readonly PassiveProfileDetailsModel _sut;
        private readonly DateTimeOffset _introductionSeenDate = DateTimeOffset.UtcNow;
        private readonly DateTimeOffset _createdDate = DateTimeOffset.UtcNow;

        public PassiveProfileDetailsModel_should()
        {
            _sut = new PassiveProfileDetailsModel()
            {
                Id = new Guid("11ff4dc8-15da-4596-8659-8337542715c2"),
                UserId = new Guid("225a5702-e0b9-4346-a96b-52a0be1e2172"),
                Email = "bbbb",
                FirstName = "bbba",
                LastName = "bbb",
                Company = "bb",
                Phone = "bbb",
                CountryCode = "bbb",
                State = "bbb",
                City = "bbb",
                PostCode = "bbb",
                Address = "b",
                Latitude = 2D,
                Longitude = 3D,
                DistanceUnit = "m",
                MassUnit = "p",
                CurrencyCode = "cur",
                DeactivationDate = DateTimeOffset.UtcNow,
                DeactivationReason = "something",
                CreatedDate = _createdDate
            };
        }

        [Fact]
        public void have_Id_property()
        {
            _sut.Id.Should().NotBeEmpty();
        }

        [Fact]
        public void have_UserId_property()
        {
            _sut.UserId.Should().NotBeEmpty();
        }

        [Fact]
        public void have_Email_property()
        {
            _sut.Email.Should().NotBeNull();
        }

        [Fact]
        public void have_FirstName_property()
        {
            _sut.FirstName.Should().Be("bbba");
        }

        [Fact]
        public void have_LastName_property()
        {
            _sut.LastName.Should().Be("bbb");
        }

        [Fact]
        public void have_Company_property()
        {
            _sut.Company.Should().Be("bb");
        }

        [Fact]
        public void have_Phone_property()
        {
            _sut.Phone.Should().Be("bbb");
        }

        [Fact]
        public void have_CountryCode_property()
        {
            _sut.CountryCode.Should().Be("bbb");
        }

        [Fact]
        public void have_State_property()
        {
            _sut.State.Should().Be("bbb");
        }

        [Fact]
        public void have_City_property()
        {
            _sut.City.Should().Be("bbb");
        }

        [Fact]
        public void have_PostCode_property()
        {
            _sut.PostCode.Should().Be("bbb");
        }

        [Fact]
        public void have_Address_property()
        {
            _sut.Address.Should().Be("b");
        }

        [Fact]
        public void have_Latitude_property()
        {
            _sut.Latitude.Should().Be(2D);
        }

        [Fact]
        public void have_Longitude_property()
        {
            _sut.Longitude.Should().Be(3D);
        }

        [Fact]
        public void have_DistanceUnit_property()
        {
            _sut.DistanceUnit.Should().Be("m");
        }

        [Fact]
        public void have_MassUnit_property()
        {
            _sut.MassUnit.Should().Be("p");
        }

        [Fact]
        public void have_CurrencyCode_property()
        {
            _sut.CurrencyCode.Should().Be("cur");
        }

        [Fact]
        public void have_DeactivationDate_property()
        {
            _sut.DeactivationDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
        }

        [Fact]
        public void have_DeactivationReason_property()
        {
            _sut.DeactivationReason.Should().Be("something");
        }

        [Fact]
        public void have_CreatedDate_property()
        {
            _sut.CreatedDate.Should().BeCloseTo(_createdDate);
        }
    }
}
