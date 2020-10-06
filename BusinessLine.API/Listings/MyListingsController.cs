﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLine.API.Listings
{
    [ApiController]
    [Route("api/mylistings")]
    public class MyListingsController : ControllerBase
    {
        [HttpGet(Name = "GetMyListings")]
        public IActionResult GetMyListings()
        {
            return Ok("my listings");
        }
    }
}
