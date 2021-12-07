using System;
using System.ComponentModel.DataAnnotations;

namespace CoursesPlatform.Entity_Framework.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }
    }
}
