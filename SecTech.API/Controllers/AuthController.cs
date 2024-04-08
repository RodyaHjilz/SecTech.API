using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecTech.Domain.Dto.User;
using SecTech.Domain.Entity;
using SecTech.Domain.Interfaces.Services;
using SecTech.Domain.Result;
using System.Security.Claims;

namespace SecTech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult<BaseResult<Token>>> Login(string email, string password)
        {
            var token = await _userService.Login(email, password);

            HttpContext.Response.Cookies.Append("token", token.Data.AccessToken);
            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<ActionResult<BaseResult<RegisterUserDto>>> Register([FromBody] RegisterUserDto user)
        {
            var response = await _userService.Register(user.Email, user.Password);
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }
        [Authorize]
        [HttpGet("info")]
        public async Task<ActionResult> GetUserInfo()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var List = new List<string> { id, email };
            return Ok(List);
            
        }


    }
}
