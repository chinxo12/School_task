using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace school.Models
{
    public class Class
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClassId { get; set; }
        [MaxLength(50, ErrorMessage = "Tên không được vượt quá 50 ký tự!")]
        public string ClassName { get; set; }
        [Range(0, int.MaxValue,ErrorMessage ="Vui lòng nhập sức chứa lớn hơn 0")]
        public int Capacity { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatorId { get; set; }
        public int FacultyId { get; set; }
        [ForeignKey("FacultyId")]
        public Faculty Faculty { get; set; }
        public List<User> Students { get; set; }
    }
}
