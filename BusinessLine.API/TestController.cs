using BusinessLine.Core.Application.Profiles.Commands.CreateProfile;
using BusinessLine.Core.Application.Profiles.Commands.MarkProfileAsIntroduced;
using BusinessLine.Core.Application.Profiles.Queries.GetProfileDetails;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLine.API
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
                Company = null,
                Phone = "+333 111 22222",
                CountryCode = "dd",
                State = null,
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
                ProfileId = new Guid("45F0C71B-0202-442C-84BD-812AFCC466E7")
            };

            _markProfileAsIntroducedCommand.Execute(parameters);

            return Ok("Hi");
        }
    }
}
