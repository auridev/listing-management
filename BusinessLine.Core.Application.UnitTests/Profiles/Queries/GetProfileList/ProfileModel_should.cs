using BusinessLine.Core.Application.Profiles.Queries.GetProfileList;
using FluentAssertions;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Profiles.Queries.GetProfileList
{
    public class ProfileModel_should
    {
        private readonly ProfileModel _sut;

        public ProfileModel_should()
        {
            _sut = new ProfileModel()
            {
                Id = Guid.NewGuid(),
                Email = "aaa@aaa.com",
                FirstName = "a",
                LastName = "b",
                City = "c",
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
        public void have_a_City_property()
        {
            _sut.City.Should().NotBeEmpty();
        }

        [Fact]
        public void have_an_IsActive_property()
        {
            _sut.IsActive.Should().Be(true);
        }
    }
}
