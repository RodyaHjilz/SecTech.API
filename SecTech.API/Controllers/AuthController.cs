﻿using Microsoft.AspNetCore.Authorization;
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
        /// Аутентификация пользователя (JWT Cookie)
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult<BaseResult<Token>>> Login([FromBody] AuthUserDto user)
        {
            var token = await _userService.Login(user.Email, user.Password);
            if (token.IsSuccess)
            {
                HttpContext.Response.Cookies.Append("token", token.Data.AccessToken);
                return Ok(token);
            }
            return BadRequest(token);
        }

        /// <summary>
        /// Регистрация пользователя в БД
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ActionResult<BaseResult>> Register([FromBody] AuthUserDto user)
        {
            var response = await _userService.Register(user.Email, user.Password);
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }


        /// <summary>
        /// Инфо об авторизированном пользователе (Не нужно)
        /// </summary>
        /// <returns></returns>
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
