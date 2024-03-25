using SecTech.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecTech.Domain.Entity
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public EventType Type { get; set; }
        

        public DateTime EventTimeStart { get; set; } // Начало мероприятия
        public DateTime EventTimeEnd { get; set; } // Конец мероприятия


        public ICollection<Attendance>? Attendances { get; set; }
    }
}
    