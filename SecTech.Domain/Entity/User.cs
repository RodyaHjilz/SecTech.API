using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
        public ICollection<Attendance>? Attendances { get; set; } // История посещений пользователя
        
    }
}
