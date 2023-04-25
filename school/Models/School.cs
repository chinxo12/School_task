using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace school.Models
{
    public class School
    {
        [Key]
        public int SchoolId { get; set; }

        public string SchoolName { get; set; }
        public DateTime FoundedTime { get; set; }
        public int Capacity { get; set; }
        public string Address { get; set; }
        public int CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public User Creator { get; set; }
        public List<Faculty> Faculties { get; set; }
    }
}
