using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SecTech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        [HttpGet("List")]
        public IActionResult GetEventsList()
        {
            return Ok();
        }

        [HttpPost("Create")]
        public IActionResult CreateEvent()
        {
            return BadRequest();
        }

        [HttpGet("Get")]
        public IActionResult GetEventById(int id)
        {
            return Ok();
        }

    }
}
