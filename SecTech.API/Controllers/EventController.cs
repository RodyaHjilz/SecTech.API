using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecTech.Domain.Dto.Event;
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
        [HttpGet("getevent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BaseResult<Event>>> GetEventById(Guid eventId)
        {
            var response = await _eventService.GetEventByIdAsync(eventId);
            if(response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
        }
    }
}
