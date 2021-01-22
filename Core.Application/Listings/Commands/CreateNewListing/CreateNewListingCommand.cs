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
using static Common.Helpers.Result;
using static LanguageExt.Prelude;
using static Common.Helpers.Functions;

namespace Core.Application.Listings.Commands.CreateNewListing
{
    public sealed class CreateNewListingCommand : ICreateNewListingCommand
    {
        private readonly IListingRepository _listingRepository;
        private readonly INewListingFactory _listingFactory;
        private readonly IDateTimeService _dateTimeService;
        private readonly IImagePersistenceService _imageRepository;
        private readonly IListingImageReferenceFactory _listingImageReferenceFactory;

        public CreateNewListingCommand(IListingRepository listingRepository,
            INewListingFactory listingFactory,
            IDateTimeService dateTimeService,
            IImagePersistenceService imageRepository,
            IListingImageReferenceFactory listingImageReferenceFactory)
        {
            _listingRepository = listingRepository ??
                throw new ArgumentNullException(nameof(listingRepository));
            _listingFactory = listingFactory ??
                throw new ArgumentNullException(nameof(listingFactory));
            _dateTimeService = dateTimeService ??
                throw new ArgumentNullException(nameof(dateTimeService));
            _imageRepository = imageRepository ??
                throw new ArgumentNullException(nameof(imageRepository));
            _listingImageReferenceFactory = listingImageReferenceFactory ??
                throw new ArgumentNullException(nameof(listingImageReferenceFactory));
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

        public Either<Error, Unit> Execute(Guid userId, CreateNewListingModel model)
        {
            Either<Error, Guid> eitherUserId = EnsureNonDefault(userId);
            Either<Error, CreateNewListingModel> eitherModel = EnsureNotNull(model);

            // Value objects
            DateTimeOffset creationDate = _dateTimeService.GetCurrentUtcDateTime();
            Either<Error, Owner> eitherOwner = Owner.Create(userId);
            Either<Error, DateTag> eitherDateTag = DateTag.Create(creationDate);
            Either<Error, ListingDetails> eitherListingDetails =
                ListingDetails
                    .Create(model.Title, model.MaterialTypeId, model.Weight, model.MassUnit, model.Description);
            Either<Error, ContactDetails> eitherContactDetails =
                ContactDetails
                    .Create(model.FirstName, model.LastName, model.Company, model.Phone);
            Either<Error, LocationDetails> eitherLocationDetails =
                LocationDetails
                    .Create(model.CountryCode, model.State, model.City, model.PostCode, model.Address);
            Either<Error, GeographicLocation> eitherGeographicLocation =
                GeographicLocation
                    .Create(model.Latitude, model.Longitude);

            // Listing
            Either<Error, NewListing> eitherListing =
                CreateListing(
                    eitherOwner,
                    eitherListingDetails,
                    eitherContactDetails,
                    eitherLocationDetails,
                    eitherGeographicLocation,
                    creationDate);

            Either<Error, (NewListing listing, CreateNewListingModel model)> combined =
                from l in eitherListing
                from m in eitherModel
                select (l, m);


            Either<Error, List<Either<Error, ImageContext>>> eitherImageContexts = combined.Map(x =>
            {
                return x.model.Images.Select(imageModel =>
                {
                    var id = Guid.NewGuid();
                    var fileInfo = new FileInfo(imageModel.Name);
                    var fileName = $"{ Guid.NewGuid()}{fileInfo.Extension }";

                    return ImageContext.Create(id, x.listing.Id, fileName, imageModel.Content);
                })
                .ToList();
            });


            Either<Error, Unit> aaa =
                (
                    from l in eitherListing
                    from ic in eitherImageContexts
                    select(l, ic)
                )
                .Bind<Unit>(context =>
                {
                    var imageContexts = context.ic.Freeze();

                    IEnumerable<ImageContext> contexts = imageContexts.Rights();
                    var contents = contexts.Select(c => c.Content).ToList();
                    var references = contexts.Select(c => c.Reference).ToList();



                    _imageRepository.AddAndSave(context.l.Id, contents, (DateTag)eitherDateTag);
                    _listingRepository.Add(context.l, references);
                    _listingRepository.Save();


                    return unit;
                });

            return aaa;

        }
    }
}
