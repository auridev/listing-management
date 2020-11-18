using Core.Application.Listings.Commands.CreateNewListing;
using FluentAssertions;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.CreateNewListing
{
    public class CreateNewListingModel_should
    {
        private readonly CreateNewListingModel _sut;

        public CreateNewListingModel_should()
        {
            _sut = new CreateNewListingModel()
            {
                Title = "title",
                MaterialTypeId = 10,
                Weight = 2.3F,
                MassUnit = "kg",
                Description = "description",
                FirstName = "firstname",
                LastName = "lasname",
                Company = "cccc",
                Phone = "+333 111 22222",
                CountryCode = "dddd",
                State = "45",
                City = "obeliai",
                PostCode = "12",
                Address = "asd",
                Latitude = 1.1D,
                Longitude = 2.2D,
                Images = new NewImageModel [] 
                {
                    new NewImageModel()
                    {
                        Name = "photo1.bmp",
                        Content = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50 }
                    },
                    new NewImageModel()
                    {
                        Name = "photo2.jpg",
                        Content = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50 }
                    }
                }
            };
        }

        [Fact]
        public void have_a_Title_property()
        {
            _sut.Title.Should().Be("title");
        }

        [Fact]
        public void have_a_MaterialTypeId_property()
        {
            _sut.MaterialTypeId.Should().Be(10);
        }

        [Fact]
        public void have_a_Weight_property()
        {
            _sut.Weight.Should().Be(2.3F);
        }

        [Fact]
        public void have_a_MassUnit_property()
        {
            _sut.MassUnit.Should().Be("kg");
        }

        [Fact]
        public void have_a_Description_property()
        {
            _sut.Description.Should().Be("description");
        }

        [Fact]
        public void have_a_FirstName_property()
        {
            _sut.FirstName.Should().Be("firstname");
        }

        [Fact]
        public void have_a_LastName_property()
        {
            _sut.LastName.Should().Be("lasname");
        }

        [Fact]
        public void have_a_Company_property()
        {
            _sut.Company.Should().Be("cccc");
        }

        [Fact]
        public void have_a_Phone_property()
        {
            _sut.Phone.Should().Be("+333 111 22222");
        }

        [Fact]
        public void have_a_CountryCode_property()
        {
            _sut.CountryCode.Should().Be("dddd");
        }

        [Fact]
        public void have_a_State_property()
        {
            _sut.State.Should().Be("45");
        }

        [Fact]
        public void have_a_City_property()
        {
            _sut.City.Should().Be("obeliai");
        }

        [Fact]
        public void have_a_PostCode_property()
        {
            _sut.PostCode.Should().Be("12");
        }

        [Fact]
        public void have_an_Address_property()
        {
            _sut.Address.Should().Be("asd");
        }

        [Fact]
        public void have_a_Latitude_property()
        {
            _sut.Latitude.Should().Be(1.1);
        }

        [Fact]
        public void have_a_Longitude_property()
        {
            _sut.Longitude.Should().Be(2.2);
        }

        [Fact]
        public void have_Images_property()
        {
            _sut.Images.Should().NotBeNull();
        }
    }
}
