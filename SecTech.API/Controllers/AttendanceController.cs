using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecTech.API.RabbitMq;
using SecTech.Domain.Dto.Event;
using SecTech.Domain.Dto.User;
using SecTech.Domain.Entity;
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
        private readonly IRabbitMqService _rabbitMqService;
        public AttendanceController(IAttendanceService attendanceService,
                                    IQRCodeService qRCodeService,
                                    IRabbitMqService rabbitMqService)
        {
            _attendanceService = attendanceService;
            _qRCodeService = qRCodeService;
            _rabbitMqService = rabbitMqService;
        }

        /// <summary>
        /// Тестовая ручка для отправки сообщения в RabbitMQ. Удалить позже
        /// </summary>
        /// <returns></returns>
        [HttpGet("testrabbit")]
        public ActionResult TestRabbit()
        {
            _rabbitMqService.SendMessage(new { Message = "Hello, World!", DateTime = DateTime.Now });
            return Ok();
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
            var response = _qRCodeService.GenerateQRCode(eventId);
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResult<Attendance>>> CheckIn(string token)
        {
            var decodeToken = _qRCodeService.DecodeQRCode(token);
            if (decodeToken.IsSuccess)
            {
                Guid userId;
                Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
                var response = await _attendanceService.CheckInAsync(userId, decodeToken.Data);
                if (response.IsSuccess)
                    return Ok(response);

                return StatusCode(500, response);
            }
            return StatusCode(500, decodeToken);
        }

        /// <summary>
        /// Возврат списка посещенных событий аутентифицированного пользователя
        /// </summary>
        /// <returns></returns>
        [HttpGet("user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResult<IEnumerable<AttendedEventDto>>>> GetUserAttendances()
        {
            Guid userId;
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
            var response = await _attendanceService.GetUserAttendancesAsync(userId);
            if (response.IsSuccess)
                return Ok(response);

            return StatusCode(500, response);

        }


        /// <summary>
        /// Возврат списка пользователей, посетивших событие
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpGet("{eventId}/users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResult<IEnumerable<AttendedUserDto>>>> GetUserAttendances(Guid eventId)
        {
            var response = await _attendanceService.GetEventAttendancesAsync(eventId);
            if (response.IsSuccess)
                return Ok(response);

            return StatusCode(500, response);

        }



    }
}
