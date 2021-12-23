using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoursesPlatform.EntityFramework.Models
{
    public class ScheduleHangfireJob
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string JobId { get; set; }

        [ForeignKey("Course")]
        public int UserSubscriptionId { get; set; }

        public virtual UserSubscriptions UserSubscription { get; set; }
    }
}
