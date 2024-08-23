using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SecTech.Application.Services;
using SecTech.Domain.Result;
using Xunit;

namespace SecTech.Tests.Services
{
    public class QRCodeServiceTests
    {
        private readonly Mock<ILogger<QRCodeService>> _mockLogger;
        private readonly QRCodeService _qrCodeService;

        public QRCodeServiceTests()
        {
            _mockLogger = new Mock<ILogger<QRCodeService>>();
            _qrCodeService = new QRCodeService(_mockLogger.Object);
        }

        [Fact]
        public void GenerateQRCode_ReturnsValidToken()
        {
            // Arrange
            var eventId = Guid.NewGuid();

            // Act
            var qrCodeUrl = _qrCodeService.GenerateQRCode(eventId);

            // Assert
            Assert.NotNull(qrCodeUrl);
            Assert.Contains("https://localhost:7163/api/Attendance/checkin/", qrCodeUrl);
        }

        [Fact]
        public void DecodeQRCode_ReturnsValidEventId()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var qrCodeUrl = _qrCodeService.GenerateQRCode(eventId);
            var tokenString = qrCodeUrl.Split('/').Last();

            // Act
            var result = _qrCodeService.DecodeQRCode(tokenString);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(eventId, result.Data);
        }

        [Fact]
        public void DecodeQRCode_ReturnsErrorForInvalidToken()
        {
            // Arrange
            var invalidToken = "invalidtoken";

            // Act
            var result = _qrCodeService.DecodeQRCode(invalidToken);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }
    }
}