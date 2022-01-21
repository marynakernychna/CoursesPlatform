using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Models.Courses;
using System.Linq;

namespace CoursesPlatform.Interfaces.Commands
{
    public interface ICoursesCommands
    {
        void CreateCourse(Course course);

        void UpdateCourse(Course course, CourseDTO newInfo);

        void DeleteCourse(Course course);

        void CreateSubscription(UserSubscriptions subscription);

        void DeleteUserSubscription(UserSubscriptions userSubscription);

        void DeleteUserSubscriptions(IQueryable<UserSubscriptions> userSubscriptions);
    }
}
