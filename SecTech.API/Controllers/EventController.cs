using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecTech.Application.Services;
using SecTech.Domain.Dto.Event;
using SecTech.Domain.Dto.User;
using SecTech.Domain.Entity;
using SecTech.Domain.Interfaces.Services;
using SecTech.Domain.Result;

namespace SecTech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }


        /// <summary>
        /// Возврат информации о событии по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("event")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BaseResult<EventDto>>> GetEventById(Guid eventId)
        {
            var response = await _eventService.GetEventByIdAsync(eventId);
            if(response.IsSuccess)
                return Ok(response);
            return StatusCode(500, response);
        }

        /// <summary>
        /// Создание мероприятия
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("event")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<BaseResult<CreateEventDto>>> CreateEvent([FromBody] CreateEventDto dto)
        {
            var response = await _eventService.CreateEventAsync(dto);
            if (response.IsSuccess)
                return Ok(response);

            return StatusCode(500, response);
        }


    }
}
