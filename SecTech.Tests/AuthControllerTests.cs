using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;
using SecTech.API.Controllers;
using SecTech.Domain.Dto.User;
using SecTech.Domain.Entity;
using SecTech.Domain.Interfaces.Services;
using SecTech.Domain.Result;
using Xunit;

namespace SecTech.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new AuthController(_mockUserService.Object);
        }

        [Fact]
        public async Task Login_ReturnsOkResult_WhenSuccess()
        {
            // Arrange
            var user = new AuthUserDto("test@example.com", "password");
            var tokenResult = new BaseResult<Token> { Data = new Token { AccessToken = "testtoken" } };
            _mockUserService.Setup(service => service.Login(user.Email, user.Password)).ReturnsAsync(tokenResult);

            // Act
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            var response = await _controller.Login(user);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            Assert.Equal(tokenResult, okResult.Value);
        }

        [Fact]
        public async Task Login_ReturnsBadRequest_WhenFailure()
        {
            // Arrange
            var user = new AuthUserDto("test@example.com", "password");
            var tokenResult = new BaseResult<Token> { ErrorMessage = "SomeMsg" };
            _mockUserService.Setup(service => service.Login(user.Email, user.Password)).ReturnsAsync(tokenResult);

            // Act
            var response = await _controller.Login(user);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(response.Result);
            Assert.Equal(tokenResult, badRequestResult.Value);
        }

        [Fact]
        public async Task Register_ReturnsOkResult_WhenSuccess()
        {
            // Arrange
            var user = new AuthUserDto("test@example.com", "password");
            var registerResult = new BaseResult<User>() { ErrorMessage = null };
            _mockUserService.Setup(service => service.Register(user.Email, user.Password)).ReturnsAsync(registerResult);

            // Act
            var response = await _controller.Register(user);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            Assert.Equal(registerResult, okResult.Value);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenFailure()
        {
            // Arrange
            var user = new AuthUserDto("test@example.com", "password");
            var registerResult = new BaseResult<User> { ErrorMessage = "SomeError" };
            _mockUserService.Setup(service => service.Register(user.Email, user.Password)).ReturnsAsync(registerResult);

            // Act
            var response = await _controller.Register(user);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(response.Result);
            Assert.Equal(registerResult, badRequestResult.Value);
        }
    }
}