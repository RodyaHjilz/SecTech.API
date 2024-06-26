﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecTech.Application.Services;
using SecTech.Domain.Dto.Event;
using SecTech.Domain.Dto.Group;
using SecTech.Domain.Dto.User;
using SecTech.Domain.Entity;
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
        private readonly IGroupService _groupService;
        private readonly IAttendanceService _attendanceService;
        public AdminController(IEventService eventService,
                               IUserService userService,
                               IGroupService groupService,
                               IAttendanceService attendanceService)
        {
            _eventService = eventService;
            _userService = userService;
            _groupService = groupService;
            _attendanceService = attendanceService;
        }


        /// <summary>
        /// Добавление группы к пользователю по GroupName и Email
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPatch("addUserGroup")]
        public async Task<ActionResult> AddGroupToUser([FromBody] AddGroupDto dto)
        {
           var response = await _groupService.AddUserToGroup(dto.email, dto.groupName);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
            
        }

        /// <summary>
        /// Создание группы
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("createGroup")]
        public async Task<ActionResult<BaseResult<UGroupDto>>> CreateGroup([FromBody] UGroupDto dto)
        {
            var response = await _groupService.CreateGroup(dto);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
        }

        /// <summary>
        /// Создание мероприятия
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("createEvent")]
        public async Task<ActionResult<BaseResult<CreateEventDto>>> CreateEvent([FromBody] CreateEventDto dto)
        {
            var response = await _eventService.CreateEventAsync(dto);
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }


        /// <summary>
        /// Возврат списка пользователей, посетивших событие
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpPost("event")]
        [Authorize]
        public async Task<ActionResult<BaseResult<IEnumerable<AttendedUserDto>>>> GetUserAttendances(Guid eventId)
        {
            var response = await _attendanceService.GetEventAttendancesAsync(eventId);
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);

        }


    }
}
