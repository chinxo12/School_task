using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace school.Models
{
    public class School
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SchoolId { get; set; }
        [MaxLength(100,ErrorMessage ="Tên quá dài!")]
        
        public string SchoolName { get; set; }
        public DateTime FoundedTime { get; set; }
        [Range(1,1000, ErrorMessage = "Vui lòng nhập từ 1-1000!")]
        public int Capacity { get; set; }
        [MaxLength(255,ErrorMessage ="Địa chỉ quá dài!")]
        public string Address { get; set; }
        public int CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public User Creator { get; set; }
        public List<Faculty> Faculties { get; set; }
    }
}
