using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecTech.Domain.Dto.Event;
using SecTech.Domain.Dto.User;
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

        /// <summary>
        /// Посещение event аутентифицированного пользователя
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("checkin/{eventId}")]
        public async Task<ActionResult<BaseResult<Attendance>>> CheckIn(Guid eventId)
        {
            Guid userId;
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
            var response = await _attendanceService.CheckInAsync(userId, eventId);
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }


        /// <summary>
        /// Возврат списка посещенных событий аутентифицированного пользователя
        /// </summary>
        /// <returns></returns>
        [HttpGet("user")]
        public async Task<ActionResult<BaseResult<IEnumerable<AttendedEventDto>>>> GetUserAttendances()
        {
            Guid userId;
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
            var response = await _attendanceService.GetUserAttendancesAsync(userId);
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);

        }

        


    }
}
