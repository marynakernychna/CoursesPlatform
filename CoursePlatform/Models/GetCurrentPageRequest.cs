using CoursesPlatform.Models.Courses;

namespace CoursesPlatform.Models.Users
{
    public class GetCurrentPageRequest
    {
        public string SearchText { get; set; }

        public FilterQuery FilterQuery { get; set; }
    }
}
