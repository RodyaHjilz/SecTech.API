using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecTech.Domain.Dto.Event;
using SecTech.Domain.Entity;
using SecTech.Domain.Interfaces.Services;
using SecTech.Domain.Result;

namespace SecTech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <returns></returns>
        [HttpGet("List")]
        public IActionResult GetEventsList()
        {
            return Ok();
        }


        /// <summary>
        /// Создание мероприятия
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Create")]
        public async Task<ActionResult<BaseResult<CreateEventDto>>> CreateEvent([FromBody] CreateEventDto dto)
        {
            var response = await _eventService.CreateEventAsync(dto);
            if(response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getevent")]
        public async Task<ActionResult<BaseResult<Event>>> GetEventById(Guid eventId)
        {
            var response = await _eventService.GetEventByIdAsync(eventId);
            if(response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
        }
    }
}
