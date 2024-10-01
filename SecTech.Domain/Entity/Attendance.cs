
using System.Text.Json.Serialization;

namespace SecTech.Domain.Entity
{
    public class Attendance
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        public Guid EventId { get; set; }
        [JsonIgnore]
        public Event Event { get; set; }

        public DateTime CheckInTime { get; set; }

    }
}
