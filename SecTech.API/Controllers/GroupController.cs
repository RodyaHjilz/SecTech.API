using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecTech.Domain.Dto.Group;
using SecTech.Domain.Interfaces.Services;
using SecTech.Domain.Result;

namespace SecTech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }


        /// <summary>
        /// Создание группы
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("group")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<BaseResult<UGroupDto>>> CreateGroup([FromBody] UGroupDto dto)
        {
            var response = await _groupService.CreateGroup(dto);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
        }

        /// <summary>
        /// Возврат списка всех групп из БД
        /// </summary>
        /// <returns></returns>
        [HttpGet("groups")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<BaseResult<IEnumerable<UGroupDto>>>> GetUGroups()
        {
            var response = await _groupService.GetGroups();
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
        }

        /// <summary>
        /// Добавление группы к пользователю по GroupName и Email
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPatch("addUserGroup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> AddGroupToUser([FromBody] AddGroupDto dto)
        {
            var response = await _groupService.AddUserToGroup(dto.email, dto.groupName);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
        }


    }
}
