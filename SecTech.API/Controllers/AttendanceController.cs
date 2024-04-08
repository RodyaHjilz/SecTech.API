using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecTech.Domain.Entity;
using SecTech.Domain.Interfaces.Services;
using SecTech.Domain.Result;
using System.Security.Claims;

namespace SecTech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [Authorize]
        [HttpGet("checkin")]
        public async Task<ActionResult<BaseResult<Attendance>>> CheckIn(Guid eventId)
        {
            Guid userId;
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
            var response = await _attendanceService.CheckInAsync(userId, eventId);
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }


        [HttpPost("user")]
        [Authorize]
        public async Task<ActionResult<BaseResult<IEnumerable<Attendance>>>> GetUserAttendances()
        {
            Guid userId;
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
            var response = await _attendanceService.GetUserAttendancesAsync(userId);
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);

        }


        [HttpPost]
        public IActionResult Post()
        {
            return Ok();
        }



    }
}
