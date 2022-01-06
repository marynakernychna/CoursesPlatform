using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Models.Courses;
using System.Collections.Generic;
using System.Linq;

namespace CoursesPlatform.Interfaces.Commands
{
    public interface ICoursesCommands
    {
        IQueryable<Course> SortCoursesByDirection(FilterQuery request, IQueryable<Course> courses);

        IQueryable<CourseDTO> FormCoursesDTOsFromCourses(IQueryable<Course> courses);

        List<User> GetSubscribersByCourseId(int courseId);

        List<CourseDTO> GetUserSubscriptions(string userId);
    }
}
