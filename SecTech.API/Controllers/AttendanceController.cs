using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SecTech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Post()
        {
            return Ok();
        }



    }
}
