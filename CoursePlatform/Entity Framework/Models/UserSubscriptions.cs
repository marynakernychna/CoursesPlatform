using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoursesPlatform.EntityFramework.Models
{
    [Table("tblUserSubscriptions")]
    public class UserSubscriptions
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public virtual User User { get; set; }
        public virtual Course Course { get; set; }
    }
}
