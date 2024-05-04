using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecTech.Domain.Dto.Group
{
    public class CreateGroupDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<string>? UserEmails { get; set; }
    }
}
