using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Models.Courses;

namespace CoursesPlatform.Models.Razor
{
    public class CourseEditingEmail
    {
        public CourseDTO NewCourseInfo { get; set; }

        public string OldTitle { get; set; }

        public string OldDescription { get; set; }

        public bool IsTitleChanged { get; set; }

        public bool IsDescriptionChanged { get; set; }

        public User User { get; set; }
    }
}
