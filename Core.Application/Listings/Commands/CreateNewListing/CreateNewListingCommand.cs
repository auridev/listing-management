using Common.Dates;
using Common.Helpers;
using Core.Application.Listings.Commands.CreateNewListing.Factory;
using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Common.Helpers.Functions;
using static LanguageExt.Prelude;

namespace Core.Application.Listings.Commands.CreateNewListing
{
    public sealed class CreateNewListingCommand : ICreateNewListingCommand
    {
        private readonly IListingRepository _listingRepository;
        private readonly INewListingFactory _listingFactory;
        private readonly IDateTimeService _dateTimeService;
        private readonly IImagePersistenceService _imageRepository;

        public CreateNewListingCommand(IListingRepository listingRepository,
            INewListingFactory listingFactory,
            IDateTimeService dateTimeService,
            IImagePersistenceService imageRepository)
        {
            _listingRepository = listingRepository ??
                throw new ArgumentNullException(nameof(listingRepository));
            _listingFactory = listingFactory ??
                throw new ArgumentNullException(nameof(listingFactory));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
            _imageRepository = imageRepository ??
                throw new ArgumentNullException(nameof(imageRepository));
        }

        private Either<Error, NewListing> CreateListing(
            Either<Error, Owner> eitherOwner,
            Either<Error, ListingDetails> eitherListingDetails,
            Either<Error, ContactDetails> eitherContactDetails,
            Either<Error, LocationDetails> eitherLocationDetails,
            Either<Error, GeographicLocation> eitherGeographicLocation,
            DateTimeOffset creationDate)
           =>
                (
                    from owner in eitherOwner
                    from listingDetails in eitherListingDetails
                    from contactDetails in eitherContactDetails
                    from locationDetails in eitherLocationDetails
                    from geographicLocation in eitherGeographicLocation
                    select
                        (owner, listingDetails, contactDetails, locationDetails, geographicLocation)
                )
                .Bind(
                    context =>
                        _listingFactory.Create(
                            context.owner,
                            context.listingDetails,
                            context.contactDetails,
                            context.locationDetails,
                            context.geographicLocation,
                            creationDate));

        private Either<Error, DateTag> CreateDateTag(DateTimeOffset createdDate)
            =>
                DateTag.Create(createdDate);

        private Either<Error, Owner> CreateOwner(Either<Error, Guid> eitherUserId)
            =>
                eitherUserId
                    .Bind(userId =>
                        Owner.Create(userId));

        private Either<Error, ListingDetails> CreateListingDetails(Either<Error, CreateNewListingModel> eitherModel)
            =>
                eitherModel
                    .Bind(model =>
                        ListingDetails.Create(
                            model.Title,
                            model.MaterialTypeId,
                            model.Weight,
                            model.MassUnit,
                            model.Description));

        private Either<Error, ContactDetails> CreateContactDetails(Either<Error, CreateNewListingModel> eitherModel)
            =>
                eitherModel
                    .Bind(model =>
                        ContactDetails.Create(
                            model.FirstName,
                            model.LastName,
                            model.Company,
                            model.Phone));

        private Either<Error, LocationDetails> CreateLocationDetails(Either<Error, CreateNewListingModel> eitherModel)
            =>
                eitherModel
                    .Bind(model =>
                        LocationDetails.Create(
                           model.CountryCode,
                           model.State,
                           model.City,
                           model.PostCode,
                           model.Address));

        private Either<Error, GeographicLocation> CreateGeographicLocation(Either<Error, CreateNewListingModel> eitherModel)
            =>
                eitherModel
                    .Bind(model =>
                        GeographicLocation.Create(
                          model.Latitude,
                          model.Longitude));

        private Either<Error, Lst<Either<Error, ImageContext>>> CreateImageContexts(Either<Error, NewListing> eitherListing, Either<Error, CreateNewListingModel> eitherModel)
            =>
                (
                    from listing in eitherListing
                    from model in eitherModel
                    select
                        (listing.Id, model)
                 )
                .Map(
                    combined =>
                        combined.model.Images
                            .Map(imageModel => CreateImageContext(combined.Id, imageModel))
                            .Freeze());

        private Either<Error, ImageContext> CreateImageContext(Guid parentReference, NewImageModel model)
        {
            var id = Guid.NewGuid();
            var fileInfo = new FileInfo(model.Name);
            var fileName = $"{ Guid.NewGuid()}{fileInfo.Extension }";

            return ImageContext.Create(id, parentReference, fileName, model.Content);
        }

        public Either<Error, Unit> Execute(Guid userId, CreateNewListingModel model)
        {
            // Prerequistites
            DateTimeOffset createdDate = _dateTimeService.GetCurrentUtcDateTime();
            Either<Error, Guid> eitherUserId = EnsureNonDefault(userId);
            Either<Error, CreateNewListingModel> eitherModel = EnsureNotNull(model);
            Either<Error, DateTag> eitherDateTag = CreateDateTag(createdDate);
            Either<Error, Owner> eitherOwner = CreateOwner(eitherUserId);
            Either<Error, ListingDetails> eitherListingDetails = CreateListingDetails(eitherModel);
            Either<Error, ContactDetails> eitherContactDetails = CreateContactDetails(eitherModel);
            Either<Error, LocationDetails> eitherLocationDetails = CreateLocationDetails(eitherModel);
            Either<Error, GeographicLocation> eitherGeographicLocation = CreateGeographicLocation(eitherModel);

            // Listing & images
            Either<Error, NewListing> eitherListing =
                CreateListing(
                    eitherOwner,
                    eitherListingDetails,
                    eitherContactDetails,
                    eitherLocationDetails,
                    eitherGeographicLocation,
                    createdDate);
            Either<Error, Lst<Either<Error, ImageContext>>> eitherImageContexts =
                CreateImageContexts(
                    eitherListing,
                    eitherModel);

            // Pesist
            Either<Error, Unit> result =
                (
                    from listing in eitherListing
                    from imageContexts in eitherImageContexts
                    from dateTag in eitherDateTag
                        select (listing, imageContexts, dateTag)
                )
                .Bind<Unit>(context =>
                {
                    IEnumerable<ImageContext> contexts = context.imageContexts.Rights();
                    List<ImageContent> contents = contexts
                        .Select(c => c.Content)
                        .ToList();
                    List<ImageReference> references = contexts
                        .Select(c => c.Reference)
                        .ToList();

                    _imageRepository.AddAndSave(context.listing.Id, contents, context.dateTag);
                    _listingRepository.Add(context.listing, references);
                    _listingRepository.Save();

                    return unit;
                });

            return result;

        }
    }
}
