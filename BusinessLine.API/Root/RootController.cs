using Microsoft.AspNetCore.Mvc;

namespace Root.API
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
