using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecTech.Domain.Dto.Event;
using SecTech.Domain.Interfaces.Services;
using SecTech.Domain.Result;

namespace SecTech.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IUserService _userService;
        public AdminController(IEventService eventService, IUserService userService)
        {
            _eventService = eventService;
            _userService = userService;
        }

        /// <summary>
        /// Создание мероприятия
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
  
        [HttpPost("Create")]
        public async Task<ActionResult<BaseResult<CreateEventDto>>> CreateEvent([FromBody] CreateEventDto dto)
        {
            var response = await _eventService.CreateEventAsync(dto);
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }
    }
}
