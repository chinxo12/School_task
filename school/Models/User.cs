using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace school.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public bool IsDeleted { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatorId { get; set; }
        public int? RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Roles Role { get; set; }
        public int? ClassId { get; set; }
        [ForeignKey("ClassId")]
        public Class Class { get; set; }
    }
}
