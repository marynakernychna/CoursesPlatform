using CoursesPlatform.EntityFramework.Models;
using System.Linq;

namespace CoursesPlatform.Interfaces.Queries
{
    public interface ICoursesQueries
    {
        IQueryable<Course> GetAllCourses();

        IQueryable<Course> GetCoursesByText(string searchText, IQueryable<Course> courses);

        IQueryable<Course> GetUserSubscriptionsById(string userId);

        Course GetCourseById(int courseId);

        IQueryable<UserSubscriptions> GetUserSubscriptionsQueryByCourseId(int courseId);

        bool CheckIsSubscriptionExists(int courseId, string userId);

        bool CheckIsCourseExistsById(int courseId);

        UserSubscriptions GetUserSubscription(string userId, int courseId);

        IQueryable<UserSubscriptions> GetUserSubscriptions(string userId);
    }
}
