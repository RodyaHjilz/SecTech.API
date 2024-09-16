using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SecTech.Domain.Interfaces.Services;
using SecTech.Domain.Result;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SecTech.Application.Services
{
    public class QRCodeService : IQRCodeService
    {
        private readonly ILogger<QRCodeService> _logger;
        private readonly IEventService _eventService;
        private readonly string _secretKey;

        public QRCodeService(ILogger<QRCodeService> logger, IEventService eventService)
        {
            _logger = logger;
            _eventService = eventService;
        }

        public BaseResult<Guid> DecodeQRCodeAsync(string tokenString)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("supersecretkeyyoooufhdshfsdh36761278fdshsdjfsa46");
            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(tokenString, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return new BaseResult<Guid>() { ErrorMessage = "Invalid token" };

                var eventIdClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "eventId");
                if (eventIdClaim == null)
                    return new BaseResult<Guid>() { ErrorMessage = "Invalid token" };

                var eventId = Guid.Parse(eventIdClaim.Value);
                var expiry = jwtToken.ValidTo;
                if(expiry < DateTime.UtcNow) { return new BaseResult<Guid>() { ErrorMessage = "QRCode expires" }; }

                return new BaseResult<Guid>() { Data = eventId };
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"Error decoding QRCode with token {tokenString}");
                return new BaseResult<Guid>() { ErrorMessage = e.Message };
            }
        }

        public string GenerateQRCodeAsync(Guid eventId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("supersecretkeyyoooufhdshfsdh36761278fdshsdjfsa46");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("eventId", eventId.ToString()) }),
                Expires = DateTime.UtcNow.AddSeconds(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return $"https://localhost:7163/api/Attendance/checkin/{tokenString}";
        }
    }
}
