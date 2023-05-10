using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace school.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [MaxLength(50,ErrorMessage ="Tên không được vượt quá 50 ký tự!")]
        public string Username { get; set; }
        [MaxLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự!")]
        public string FullName { get; set; }
        [MaxLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự!")]
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        [MaxLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự!")]
        public string Address { get; set; }
        public bool IsDeleted { get; set; }
        [MaxLength(100)]
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
