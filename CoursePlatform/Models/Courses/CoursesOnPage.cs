using CoursesPlatform.EntityFramework.Models;
using System.Collections.Generic;

namespace CoursesPlatform.Models.Courses
{
    public class CoursesOnPage
    {
        public int TotalCount { get; set; }

        public List<Course> Courses { get; set; }
    }
}
