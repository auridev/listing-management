using Core.Application.Listings.Commands.CreateNewListing.Factory;
using Core.Domain.Common;
using Core.Domain.Listings;
using Common.Dates;
using System;
using System.IO;
using System.Linq;

namespace Core.Application.Listings.Commands.CreateNewListing
{
    public sealed class CreateNewListingCommand : ICreateNewListingCommand
    {
        private readonly IListingRepository _repository;
        private readonly INewListingFactory _factory;
        private readonly IDateTimeService _dateTimeService;
        private readonly IImageStorageService _imageStorageService;
        private readonly IListingImageReferenceFactory _listingImageReferenceFactory;

        public CreateNewListingCommand(IListingRepository repository,
            INewListingFactory factory,
            IDateTimeService dateTimeService,
            IImageStorageService imageStorageService,
            IListingImageReferenceFactory listingImageReferenceFactory)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            _factory = factory ??
                throw new ArgumentNullException(nameof(factory)); ;
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
            _imageStorageService = imageStorageService ??
                throw new ArgumentNullException(nameof(imageStorageService));
            _listingImageReferenceFactory = listingImageReferenceFactory ??
                throw new ArgumentNullException(nameof(listingImageReferenceFactory));
        }

        public void Execute(Guid userId, CreateNewListingModel model)
        {
            // Pre-requisties
            var owner = Owner.Create(userId);

            var creationDate = _dateTimeService.GetCurrentUtcDateTime();
            var dateTag = DateTag.Create(creationDate);

            var listingDetails = ListingDetails.Create(
                Title.Create(model.Title),
                MaterialType.ById(model.MaterialTypeId),
                Weight.Create(model.Weight, MassMeasurementUnit.BySymbol(model.MassUnit)),
                Description.Create(model.Description));

            var contactDetails = ContactDetails.Create(
                PersonName.Create(model.FirstName, model.LastName),
                Company.Create(model.Company),
                Phone.Create(model.Phone));

            var locationDetails = LocationDetails.Create(
                Alpha2Code.Create(model.CountryCode),
                State.Create(model.State),
                City.Create(model.City),
                PostCode.Create(model.PostCode),
                Address.Create(model.Address));

            var geographicLocation = GeographicLocation.Create(
                model.Latitude,
                model.Longitude);

            // Create all the entities
            NewListing newListing = _factory.Create(owner,
                listingDetails,
                contactDetails,
                locationDetails,
                geographicLocation,
                creationDate);

            NewImageModel[] validImageModels =
                GetValidImageModels(model.Images);

            (FileName FileName, NewImageModel Model)[] fileNameModelMap =
                CreateFileNameModelMap(validImageModels);

            ListingImageReference[] imageReferences =
                CreateImageReferences(newListing.Id, fileNameModelMap);

            ImageContent[] imageContents =
                CreateImageContents(fileNameModelMap);

            // Save all
            _repository.Add(newListing, imageReferences);

            _imageStorageService.Save(newListing.Id, imageContents, dateTag);

            _repository.Save();
        }

        private NewImageModel[] GetValidImageModels(NewImageModel[] imageModels)
        {
            return imageModels
                .Filter(m => m.Content.Length > 0)
                .ToArray();
        }

        private (FileName FileName, NewImageModel Model)[] CreateFileNameModelMap(NewImageModel[] imageModels)
        {
            (FileName FileName, NewImageModel Model)[] map = imageModels
                .Map<NewImageModel, (FileName FileName, NewImageModel)>(model =>
                {
                    var fileInfo = new FileInfo(model.Name);
                    var uniqueFileName = $"{ Guid.NewGuid()}.{fileInfo.Extension }";
                    var fileName = FileName.Create(uniqueFileName);
                    return (fileName, model);
                })
                .ToArray();

            return map;
        }

        private ListingImageReference[] CreateImageReferences(Guid parentReference, (FileName FileName, NewImageModel Model)[] fileNameModelMap)
        {
            ListingImageReference[] references = fileNameModelMap
                .Map(mapEntry => _listingImageReferenceFactory.Create(parentReference, mapEntry.FileName, FileSize.Create(mapEntry.Model.Content.Length)))
                .ToArray();

            return references;
        }

        private ImageContent[] CreateImageContents((FileName FileName, NewImageModel Model)[] fileNameModelMap)
        {
            ImageContent[] contents = fileNameModelMap
                .Map(mapEntry => ImageContent.Create(mapEntry.FileName, mapEntry.Model.Content))
                .ToArray();

            return contents;
        }
    }
}
