using System.Text.Json.Serialization;
namespace SecTech.Domain.Entity
{
    public class User
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }

        [JsonIgnore]
        public ICollection<UGroup>? Groups { get; set; }

        // История посещений пользователя
        [JsonIgnore]
        public ICollection<Attendance>? Attendances { get; set; }
        
    }
}

