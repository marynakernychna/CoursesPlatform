using System.Collections.Generic;

namespace CoursesPlatform.Models.Courses
{
    public class SubscriptionsOnPage
    {
        public int TotalCount { get; set; }

        public List<CourseDTO> Subscriptions { get; set; }
    }
}
