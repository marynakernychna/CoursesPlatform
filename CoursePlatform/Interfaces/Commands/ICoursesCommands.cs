using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Models.Courses;
using System.Collections.Generic;
using System.Linq;

namespace CoursesPlatform.Interfaces.Commands
{
    public interface ICoursesCommands
    {
        IQueryable<Course> SortCourses(FilterQuery request, IQueryable<Course> courses);

        List<Course> GetCoursesOnPage(FilterQuery request, IQueryable<Course> courses);

        IQueryable<CourseDTO> SortSubscriptions(FilterQuery request, IQueryable<CourseDTO> courses);

        List<CourseDTO> GetUserSubscriptionsOnPage(FilterQuery request, IQueryable<CourseDTO> courses);

        List<UserSubscriptions> GetUserSubscriptionsQueryByCourseId(int courseId);

        UserSubscriptions GetUserSubscription(int courseId, string userId);

        void UnsubscribeFromCourse(string userId, int courseId);

        List<CourseDTO> GetUserSubscriptions(string userId);

    }
}
