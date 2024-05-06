using SecTech.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecTech.Domain.Dto.Group
{
    public class UGroupDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public EventType Type { get; set; }
        public List<string>? UserEmails { get; set; }
    }
}
