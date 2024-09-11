using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecTech.Domain.Dto.Event;
using SecTech.Domain.Dto.User;
using SecTech.Domain.Entity;
using SecTech.Domain.Enums;
using SecTech.Domain.Interfaces.Services;
using SecTech.Domain.Result;
using System.Security.Claims;

namespace SecTech.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IQRCodeService _qRCodeService;

        public AttendanceController(IAttendanceService attendanceService,
                                    IQRCodeService qRCodeService,
                                    ILogger<AttendanceController> logger)
        {
            _attendanceService = attendanceService;
            _qRCodeService = qRCodeService;
        }

        /// <summary>
        /// Генерация QR кода, который живет 20 секунд
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<BaseResult<string>>> GenerateQR(Guid eventId)
        {
            var response = _qRCodeService.GenerateQRCodeAsync(eventId);
            return Ok(response);
        }

        /// <summary>
        /// Посещение event аутентифицированного пользователя
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("checkin/{token}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<Attendance>>> CheckIn(string token)
        {
            var decodeToken = _qRCodeService.DecodeQRCodeAsync(token);
            if (decodeToken.IsSuccess)
            {
                Guid userId;
                Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
                var response = await _attendanceService.CheckInAsync(userId, decodeToken.Data);
                if (response.IsSuccess)
                    return Ok(response);

                return BadRequest(response);
            }
            return BadRequest(decodeToken);
        }

        /// <summary>
        /// Возврат списка посещенных событий аутентифицированного пользователя
        /// </summary>
        /// <returns></returns>
        [HttpGet("user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
