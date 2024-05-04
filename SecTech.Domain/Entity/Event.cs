using SecTech.Domain.Enums;
namespace SecTech.Domain.Entity
{
    public class Event
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public EventType Type { get; set; }
        

        public DateTime EventTimeStart { get; set; } // Начало мероприятия
        public DateTime EventTimeEnd { get; set; } // Конец мероприятия


        public ICollection<UGroup>? AccessedGroups { get; set; }
        public ICollection<Attendance>? Attendances { get; set; }
    }
}
    