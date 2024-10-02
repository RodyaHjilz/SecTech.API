using Microsoft.AspNetCore.Mvc;
using Moq;
using SecTech.API.Controllers;
using SecTech.Domain.Dto.Group;
using SecTech.Domain.Interfaces.Services;
using SecTech.Domain.Result;

namespace SecTech.Tests
{
    public class GroupCotrollerTests
    {
        private readonly GroupController _controller;
        private readonly Mock<IGroupService> _groupService;

        public GroupCotrollerTests()
        {
            _groupService = new Mock<IGroupService>();
            _controller = new GroupController(_groupService.Object);
        }


        [Fact]
        public async Task CreateGroup_ReturnsOkResult_WhenSuccess()
        {
            // Arrange
            var group = new UGroupDto() { Description = "testGroup", Name = "testGroup", Type = 0, UserEmails = new List<string>() { "testmails" } };
            var groupResult = new BaseResult<UGroupDto> { Data = group };
            _groupService.Setup(service => service.CreateGroup(group)).ReturnsAsync(groupResult);

            // Act
            var response = await _controller.CreateGroup(group);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            Assert.Equal(groupResult, okResult.Value);
        }

        [Fact]
        public async Task CreateGroup_ReturnsBadRequest_WhenFailure()
        {
            // Arrange
            var group = new UGroupDto() { Description = "testGroup", Name = "testGroup", Type = 0, UserEmails = new List<string>() { "testmails" } };
            var groupResult = new BaseResult<UGroupDto> { ErrorMessage = "SomeMsg" };
            _groupService.Setup(service => service.CreateGroup(group)).ReturnsAsync(groupResult);

            // Act
            var response = await _controller.CreateGroup(group);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(response.Result);
            Assert.Equal(groupResult, badRequestResult.Value);
        }

        [Fact]
        public async Task GetUGroups_ReturnsOkResult_WhenSuccess()
        {
            // Arrange
            var groups = new List<UGroupDto>() { new UGroupDto() { Description = "testGroup", Name = "testGroup", Type = 0, UserEmails = new List<string>() { "testmails" } } };
            var groupResult = new BaseResult<IEnumerable<UGroupDto>> { Data = groups };
            _groupService.Setup(service => service.GetGroups()).ReturnsAsync(groupResult);

            // Act
            var response = await _controller.GetUGroups();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            Assert.Equal(groupResult, okResult.Value);
        }

        [Fact]
        public async Task GetUGroups_ReturnsBadRequest_WhenFailure()
        {
            // Arrange
            var groups = new List<UGroupDto>() { new UGroupDto() { Description = "testGroup", Name = "testGroup", Type = 0, UserEmails = new List<string>() { "testmails" } } };
            var groupResult = new BaseResult<IEnumerable<UGroupDto>> { ErrorMessage = "SomeMsg" };
            _groupService.Setup(service => service.GetGroups()).ReturnsAsync(groupResult);

            // Act
            var response = await _controller.GetUGroups();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(response.Result);
            Assert.Equal(groupResult, badRequestResult.Value);
        }

        [Fact]
        public async Task AddUserGroup_ReturnsOkResult_WhenSuccess()
        {
            // Arrange
            var group = new AddGroupDto("email", "groupname");
            var groupResult = new BaseResult<UGroupDto>() { Data = new UGroupDto() { Description = "testGroup", Name = "testGroup", Type = 0, UserEmails = new List<string>() { "testmails", "testmail" } } };
            _groupService.Setup(service => service.AddUserToGroup(group.email, group.groupName)).ReturnsAsync(groupResult);

            // Act
            var response = await _controller.AddGroupToUser(group);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            Assert.Equal(groupResult, okResult.Value);
        }

        [Fact]
        public async Task AddUserGroup_ReturnsBadRequest_WhenFailure()
        {
            // Arrange
            var group = new AddGroupDto("email", "groupname");
            var groupResult = new BaseResult<UGroupDto> { ErrorMessage = "SomeMsg" };
            _groupService.Setup(service => service.AddUserToGroup(group.email, group.groupName)).ReturnsAsync(groupResult);

            // Act
            var response = await _controller.AddGroupToUser(group);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(response.Result);
            Assert.Equal(groupResult, badRequestResult.Value);
        }

    }
}
