using Core.Application.Listings.Queries.GetMyActiveListingDetails;
using Core.Application.Listings.Queries.GetMyListings;
using Core.Application.Listings.Queries.GetPublicListings;
using Core.Application.Messages.Queries.GetMyMessageDetails;
using Core.Application.Messages.Queries.GetMyMessages;
using Core.Application.Profiles.Commands.CreateProfile;
using Core.Application.Profiles.Commands.MarkProfileAsIntroduced;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API
{
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ICreateProfileCommand _createProfileCommand;
        private readonly IMarkProfileAsIntroducedCommand _markProfileAsIntroducedCommand;

        private readonly IGetMyMessageDetailsQuery _getMyMessageDetailsQuery;
        private readonly IGetMyMessagesQuery _getMyMessagesQuery;

        private readonly IGetMyActiveListingDetailsQuery _getMyActiveListingDetailsQuery;
        private readonly IGetMyListingsQuery _getMyListingsQuery;
        private readonly IGetPublicListingsQuery _getPublicListingsQuery;

        public TestController(
            ICreateProfileCommand createProfileCommand,
            IMarkProfileAsIntroducedCommand markProfileAsIntroducedCommand,
            IGetMyMessageDetailsQuery getMyMessageDetailsQuery,
            IGetMyMessagesQuery getMyMessagesQuery,
            IGetMyActiveListingDetailsQuery getMyActiveListingDetailsQuery,
            IGetMyListingsQuery getMyListingsQuery,
            IGetPublicListingsQuery getPublicListingsQuery)
        {
            _createProfileCommand = createProfileCommand;
            _markProfileAsIntroducedCommand = markProfileAsIntroducedCommand;

            _getMyMessageDetailsQuery = getMyMessageDetailsQuery;
            _getMyMessagesQuery = getMyMessagesQuery;

            _getMyActiveListingDetailsQuery = getMyActiveListingDetailsQuery;
            _getMyListingsQuery = getMyListingsQuery;

            _getPublicListingsQuery = getPublicListingsQuery;
        }


        [HttpGet("api/test/addprofile")]
        public IActionResult AddActiveProfiles()
        {
            var model = new CreateProfileModel()
            {
                Email = "one@two.com",
                FirstName = "rose",
                LastName = "mary",
                Company = "aaaa",
                Phone = "+333 111 22222",
                CountryCode = "dd",
                State = "asdasd",
                City = "utena",
                PostCode = "pcode",
                Address = "my place 1",
                Latitude = 23D,
                Longitude = 100D,
                DistanceUnit = "km",
                MassUnit = "lb",
                CurrencyCode = "eur"
            };

            _createProfileCommand.Execute(Guid.NewGuid(), model);

            return Ok();
        }

        [HttpGet("api/test/getactiveprofiles")]
        public IActionResult GetActiveProfiles()
        {
            var parameters = new MarkProfileAsIntroducedModel()
            {
                ProfileId = new Guid("70950BB0-3B5F-4567-98D2-DCF8564E2D1A")
            };

            _markProfileAsIntroducedCommand.Execute(parameters);

            return Ok("Hi");
        }




        [HttpGet("api/test/messages")]
        public IActionResult GetMessages(int pageNumber = 1, int pageSize = 10)
        {
            var userId = new Guid("6A95949C-355F-4E1F-BDCF-D4F28F12E9C8");

            var parameters = new GetMyMessagesQueryParams()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };


            var messages = _getMyMessagesQuery.Execute(userId, parameters);

            return Ok(messages);
        }

        [HttpGet("api/test/messages/{id}")]
        public IActionResult GetMessageDetails(Guid id)
        {
            var userId = new Guid("6A95949C-355F-4E1F-BDCF-D4F28F12E9C8");

            var message = _getMyMessageDetailsQuery.Execute(userId, id);

            return Ok(message);
        }

        [HttpGet("api/test/listings/active")]
        public IActionResult GetActiveDetails()
        {
            var listingId = new Guid("85E5AFBF-5850-42F3-BDEB-004A116B3914");
            var userId = new Guid("429D32D1-B64C-421E-B013-3D3932B9A654");


            var message = _getMyActiveListingDetailsQuery.Execute(userId, listingId);

            return Ok(message);
        }

        [HttpGet("api/test/listings/my")]
        public IActionResult GetMyListings()
        {

            var userId = new Guid("626D6839-CA59-405D-979A-B7BEC2BF350F");
            var parameters = new GetMyListingsQueryParams()
            {
                PageNumber = 1,
                PageSize = 20
            };


            var message = _getMyListingsQuery.Execute(userId, parameters);

            return Ok(message);
        }


        [HttpGet("api/test/listings/public")]
        public IActionResult GetPublicListings()
        {

            var userId = new Guid("429D32D1-B64C-421E-B013-3D3932B9A654");
            var parameters = new GetPublicListingsQueryParams()
            {
                OnlyWithMyOffers = false,
                PageNumber = 1,
                PageSize = 20
            };


            var message = _getPublicListingsQuery.Execute(userId, parameters);

            return Ok(message);
        }
    }
}
