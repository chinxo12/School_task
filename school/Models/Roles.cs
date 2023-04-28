using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace school.Models
{
    public class Role
    {
        [Key]
  
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
