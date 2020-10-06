using BusinessLine.Common.Dates;
using BusinessLine.Core.Application.Listings.Commands;
using BusinessLine.Core.Application.Listings.Commands.CreateNewListing;
using BusinessLine.Core.Application.Listings.Commands.CreateNewListing.Factory;
using BusinessLine.Core.Application.UnitTests.TestMocks;
using BusinessLine.Core.Domain.Common;
using BusinessLine.Core.Domain.Listings;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.CreateNewListing
{
    public class CreateNewListingCommand_should
    {
        private readonly CreateNewListingCommand _sut;
        private readonly CreateNewListingModel _model;
        private readonly NewListing _listing;
        private readonly AutoMocker _mocker;

        public CreateNewListingCommand_should()
        {
            _mocker = new AutoMocker();

            _model = new CreateNewListingModel()
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
                CountryCode = "dd",
                State = "45",
                City = "obeliai",
                PostCode = "12",
                Address = "asd",
                Latitude = 1.1D,
                Longitude = 2.2D,
                Images = new NewImageModel[]
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

            _listing = new NewListing(
                Guid.NewGuid(),
                Owner.Create(Guid.NewGuid()),
                ValueObjectMocks.ListingDetails,
                ValueObjectMocks.ContactDetails,
                ValueObjectMocks.LocationDetails,
                ValueObjectMocks.GeographicLocation,
                DateTimeOffset.UtcNow);

            _mocker
                .GetMock<INewListingFactory>()
                .Setup(factory => factory.Create(
                    It.IsAny<Owner>(),
                    It.IsAny<ListingDetails>(),
                    It.IsAny<ContactDetails>(),
                    It.IsAny<LocationDetails>(),
                    It.IsAny<GeographicLocation>(),
                    It.IsAny<DateTimeOffset>()))
                .Returns(_listing);

            _mocker
                .GetMock<IDateTimeService>()
                .Setup(service => service.GetCurrentUtcDateTime())
                .Returns(DateTimeOffset.UtcNow);

            _sut = _mocker.CreateInstance<CreateNewListingCommand>();
        }

        [Fact]
        public void add_new_listing_to_the_repository()
        {
            _sut.Execute(Guid.NewGuid(), _model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Add(_listing), Times.Once);
        }

        [Fact]
        public void save_repository_changes()
        {
            _sut.Execute(Guid.NewGuid(), _model);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Once);
        }
    }
}
