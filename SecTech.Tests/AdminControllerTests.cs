//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Moq;
//using SecTech.API.Controllers;
//using SecTech.Domain.Dto.Event;
//using SecTech.Domain.Dto.Group;
//using SecTech.Domain.Dto.User;
//using SecTech.Domain.Interfaces.Services;
//using SecTech.Domain.Result;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Xunit;

//namespace SecTech.Tests
//{
//    public class AdminControllerTests
//    {
//        private readonly Mock<IEventService> _mockEventService;
//        private readonly Mock<IUserService> _mockUserService;
//        private readonly Mock<IGroupService> _mockGroupService;
//        private readonly Mock<IAttendanceService> _mockAttendanceService;
//        private readonly Mock<ILogger<AdminController>> _mockLogger;
//        private readonly AdminController _adminController;

//        public AdminControllerTests()
//        {
//            _mockEventService = new Mock<IEventService>();
//            _mockUserService = new Mock<IUserService>();
//            _mockGroupService = new Mock<IGroupService>();
//            _mockAttendanceService = new Mock<IAttendanceService>();
//            _mockLogger = new Mock<ILogger<AdminController>>();
//            _adminController = new AdminController(
//                _mockEventService.Object,
//                _mockUserService.Object,
//                _mockGroupService.Object,
//                _mockAttendanceService.Object,
//                _mockLogger.Object
//            );
//        }

//        [Fact]
//        public async Task AddGroupToUser_ValidRequest_ReturnsOk()
//        {
//            // Arrange
//            var dto = new AddGroupDto("test@example.com", "TestGroup");
//            _mockGroupService.Setup(service => service.AddUserToGroup(dto.email, dto.groupName))
//                .ReturnsAsync(new BaseResult { ErrorMessage = null });

//            // Act
//            var result = await _adminController.AddGroupToUser(dto);

//            // Assert
//            var actionResult = Assert.IsType<OkObjectResult>(result);
//            var response = Assert.IsType<BaseResult>(actionResult.Value);
//            Assert.True(response.IsSuccess);
//        }

//        [Fact]
//        public async Task AddGroupToUser_InvalidRequest_ReturnsBadRequest()
//        {
//            // Arrange
//            var dto = new AddGroupDto { email = "test@example.com", groupName = "TestGroup" };
//            _mockGroupService.Setup(service => service.AddUserToGroup(dto.email, dto.groupName))
//                .ReturnsAsync(new BaseResult { IsSuccess = false });

//            // Act
//            var result = await _adminController.AddGroupToUser(dto);

//            // Assert
//            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
//            var response = Assert.IsType<BaseResult>(actionResult.Value);
//            Assert.False(response.IsSuccess);
//        }

//        [Fact]
//        public async Task CreateGroup_ValidRequest_ReturnsOk()
//        {
//            // Arrange
//            var dto = new UGroupDto { Name = "TestGroup" };
//            _mockGroupService.Setup(service => service.CreateGroup(dto))
//                .ReturnsAsync(new BaseResult<UGroupDto> { IsSuccess = true, Data = dto });

//            // Act
//            var result = await _adminController.CreateGroup(dto);

//            // Assert
//            var actionResult = Assert.IsType<OkObjectResult>(result);
//            var response = Assert.IsType<BaseResult<UGroupDto>>(actionResult.Value);
//            Assert.True(response.IsSuccess);
//            Assert.Equal(dto, response.Data);
//        }

//        [Fact]
//        public async Task CreateGroup_InvalidRequest_ReturnsBadRequest()
//        {
//            // Arrange
//            var dto = new UGroupDto { Name = "TestGroup" };
//            _mockGroupService.Setup(service => service.CreateGroup(dto))
//                .ReturnsAsync(new BaseResult<UGroupDto> { IsSuccess = false });

//            // Act
//            var result = await _adminController.CreateGroup(dto);

//            // Assert
//            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
//            var response = Assert.IsType<BaseResult<UGroupDto>>(actionResult.Value);
//            Assert.False(response.IsSuccess);
//        }

//        [Fact]
//        public async Task CreateEvent_ValidRequest_ReturnsOk()
//        {
//            // Arrange
//            var dto = new CreateEventDto { Name = "TestEvent" };
//            _mockEventService.Setup(service => service.CreateEventAsync(dto))
//                .ReturnsAsync(new BaseResult<CreateEventDto> { IsSuccess = true, Data = dto });

//            // Act
//            var result = await _adminController.CreateEvent(dto);

//            // Assert
//            var actionResult = Assert.IsType<OkObjectResult>(result);
//            var response = Assert.IsType<BaseResult<CreateEventDto>>(actionResult.Value);
//            Assert.True(response.IsSuccess);
//            Assert.Equal(dto, response.Data);
//        }

//        [Fact]
//        public async Task CreateEvent_InvalidRequest_ReturnsBadRequest()
//        {
//            // Arrange
//            var dto = new CreateEventDto { Name = "TestEvent" };
//            _mockEventService.Setup(service => service.CreateEventAsync(dto))
//                .ReturnsAsync(new BaseResult<CreateEventDto> { IsSuccess = false });

//            // Act
//            var result = await _adminController.CreateEvent(dto);

//            // Assert
//            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
//            var response = Assert.IsType<BaseResult<CreateEventDto>>(actionResult.Value);
//            Assert.False(response.IsSuccess);
//        }

//        [Fact]
//        public async Task GetUserAttendances_ValidRequest_ReturnsOk()
//        {
//            // Arrange
//            var eventId = Guid.NewGuid();
//            var attendedUsers = new List<AttendedUserDto> { new AttendedUserDto { Email = "test@example.com" } };
//            _mockAttendanceService.Setup(service => service.GetEventAttendancesAsync(eventId))
//                .ReturnsAsync(new BaseResult<IEnumerable<AttendedUserDto>> { IsSuccess = true, Data = attendedUsers });

//            // Act
//            var result = await _adminController.GetUserAttendances(eventId);

//            // Assert
//            var actionResult = Assert.IsType<OkObjectResult>(result);
//            var response = Assert.IsType<BaseResult<IEnumerable<AttendedUserDto>>>(actionResult.Value);
//            Assert.True(response.IsSuccess);
//            Assert.Equal(attendedUsers, response.Data);
//        }

//        [Fact]
//        public async Task GetUserAttendances_InvalidRequest_ReturnsBadRequest()
//        {
//            // Arrange
//            var eventId = Guid.NewGuid();
//            _mockAttendanceService.Setup(service => service.GetEventAttendancesAsync(eventId))
//                .ReturnsAsync(new BaseResult<IEnumerable<AttendedUserDto>> { IsSuccess = false });

//            // Act
//            var result = await _adminController.GetUserAttendances(eventId);

//            // Assert
//            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
//            var response = Assert.IsType<BaseResult<IEnumerable<AttendedUserDto>>>(actionResult.Value);
//            Assert.False(response.IsSuccess);
//        }

//        [Fact]
//        public async Task GetUGroups_ValidRequest_ReturnsOk()
//        {
//            // Arrange
//            var groups = new List<UGroupDto> { new UGroupDto { Name = "TestGroup" } };
//            _mockGroupService.Setup(service => service.GetGroups())
//                .ReturnsAsync(new BaseResult<IEnumerable<UGroupDto>> { IsSuccess = true, Data = groups });

//            // Act
//            var result = await _adminController.GetUGroups();

//            // Assert
//            var actionResult = Assert.IsType<OkObjectResult>(result);
//            var response = Assert.IsType<BaseResult<IEnumerable<UGroupDto>>>(actionResult.Value);
//            Assert.True(response.IsSuccess);
//            Assert.Equal(groups, response.Data);
//        }

//        [Fact]
//        public async Task GetUGroups_InvalidRequest_ReturnsBadRequest()
//        {
//            // Arrange
//            _mockGroupService.Setup(service => service.GetGroups())
//                .ReturnsAsync(new BaseResult<IEnumerable<UGroupDto>> { IsSuccess = false });

//            // Act
//            var result = await _adminController.GetUGroups();

//            // Assert
//            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
//            var response = Assert.IsType<BaseResult<IEnumerable<UGroupDto>>>(actionResult.Value);
//            Assert.False(response.IsSuccess);
//        }
//    }
//}