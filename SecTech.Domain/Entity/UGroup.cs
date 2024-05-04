
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SecTech.Domain.Entity
{
    
    public class UGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<User>? Users { get; set; }


    }
}
