using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace school.Models
{
    public class Faculty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FacultyId { get; set; }
       
        [DisplayName("Khoa")]
        public string FacultyName { get; set; }
        [DisplayName("Sức chứa")]
        public int Capacity { get; set; }
        [DisplayName("Ngày tạo")]
        public DateTime CreatedDate { get; set; }
        public int SchoolId { get; set; }
        [ForeignKey("SchoolId")]
        public School School { get; set; }
        public List<Class> Classes { get; set; }
    }
}
