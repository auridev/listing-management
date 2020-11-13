using Core.Application.Profiles.Commands.CreateProfile;
using Core.Application.Profiles.Commands.MarkProfileAsIntroduced;
using Core.Application.Profiles.Queries.GetProfileDetails;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ICreateProfileCommand _createProfileCommand;
        private readonly IMarkProfileAsIntroducedCommand _markProfileAsIntroducedCommand;

        public TestController(
            ICreateProfileCommand createProfileCommand,
            IMarkProfileAsIntroducedCommand markProfileAsIntroducedCommand)
        {
            _createProfileCommand = createProfileCommand;
            _markProfileAsIntroducedCommand = markProfileAsIntroducedCommand;
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
    }
}
