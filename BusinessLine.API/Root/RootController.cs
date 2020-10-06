using BusinessLine.Core.Application;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLine.API.Root
{
    [Route("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot()
        {
            return Ok("Hi");
        }
    }
}
