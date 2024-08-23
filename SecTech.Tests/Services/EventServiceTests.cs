using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using SecTech.Application.Services;
using SecTech.Domain.Dto.Event;
using SecTech.Domain.Entity;
using SecTech.Domain.Interfaces.Repositories;
using SecTech.Domain.Result;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace SecTech.Tests
{
    public class EventServiceTests
    {
        private readonly Mock<IBaseRepository<Event>> _mockEventRepository;
        private readonly EventService _eventService;

        public EventServiceTests()
        {
            _mockEventRepository = new Mock<IBaseRepository<Event>>();
            _eventService = new EventService(_mockEventRepository.Object);
        }

        [Fact]
        public async Task CreateEventAsync_NullEventDto_ReturnsErrorMessage()
        {
            // Act
            var result = await _eventService.CreateEventAsync(null);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Entity is null", result.ErrorMessage);
        }

        [Fact]
        public async Task CreateEventAsync_ValidEventDto_ReturnsEvent()
        {
            // Arrange
            var eventDto = new CreateEventDto
            {
                Address = "123 Main St",
                Description = "Test Event",
                Name = "Event Name",
                Type = 0,
                EventTimeEnd = DateTime.UtcNow.AddHours(2),
                EventTimeStart = DateTime.UtcNow
            };
            _mockEventRepository.Setup(repo => repo.CreateAsync(It.IsAny<Event>()))
                .ReturnsAsync((Event evnt) => evnt);

            // Act
            var result = await _eventService.CreateEventAsync(eventDto);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(eventDto.Name, result.Data.Name);
        }

    }
}