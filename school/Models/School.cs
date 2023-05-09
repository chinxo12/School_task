﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace school.Models
{
    public class School
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SchoolId { get; set; }
        [MaxLength(50)]
        public string SchoolName { get; set; }
        public DateTime FoundedTime { get; set; }
        
        public int Capacity { get; set; }
        [MaxLength(255)]
        public string Address { get; set; }
        public int CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public User Creator { get; set; }
        public List<Faculty> Faculties { get; set; }
    }
}
