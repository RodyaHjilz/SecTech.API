using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SecTech.Domain.Dto.User;
using SecTech.Domain.Entity;
using SecTech.Domain.Interfaces.Repositories;
using SecTech.Domain.Interfaces.Services;
using SecTech.Domain.Result;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SecTech.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _userRepository;

        public UserService(IBaseRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<BaseResult<Token>> Login(string email, string password)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Email == email);
                if (user == null)
                    return new BaseResult<Token>()
                    {
                        ErrorMessage = "User is not found"
                    };


                // TODO: Проверка пароля

                var claims = new List<Claim> { 
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };
                var jwt = new JwtSecurityToken(
                issuer: "SecTech",
                audience: "SecTech",
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysupersecret_secretsecretsecretkey!123")),
                SecurityAlgorithms.HmacSha256));


                var token = new Token()
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt)
                };
                return new BaseResult<Token>()
                {
                    Data = token
                };
            }
            catch (Exception ex)
            {
                return new BaseResult<Token>() { ErrorMessage = ex.Message };
            }
        }
        // TODO: Добавить хеширование пароля
        public async Task<BaseResult<User>> Register(string email, string password)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Email == email);
                if (user != null)
                    return new BaseResult<User> { Data = user, ErrorMessage = "User are already exists" };

                user = new User()
                {
                    Email = email,
                    Password = password,
                    Id = Guid.NewGuid()
                };
                await _userRepository.CreateAsync(user);
                return new BaseResult<User> { Data = user };
            }
            catch (Exception ex)
            {
                return new BaseResult<User>()
                {
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }
        }
    }
}
