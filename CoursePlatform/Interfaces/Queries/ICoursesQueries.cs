using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Models.Courses;
using System.Linq;

namespace CoursesPlatform.Interfaces.Queries
{
    public interface ICoursesQueries
    {
        IQueryable<Course> SearchTextInCourses(string searchText, IQueryable<Course> courses);

        IQueryable<Course> GetAllCourses();

        IQueryable<Course> GetUserSubscriptionsById(string userId);

        Course GetCourseById(int courseId);

        void AddCourse(Course course);

        IQueryable<UserSubscriptions> GetUserSubscriptionsQueryByCourseId(int courseId);

        void UpdateCourse(Course course, CourseDTO newInfo);

        void RemoveCourse(Course course);

        bool CheckIsSubscriptionExists(int courseId, string userId);

        void AddSubscription(UserSubscriptions subscription);

        bool CheckIsCourseExistsById(int courseId);

        UserSubscriptions GetUserSubscription(string userId, int courseId);

        void RemoveUserSubscription(UserSubscriptions userSubscription);

        IQueryable<UserSubscriptions> GetUserSubscriptions(string userId);

        void RemoveUserSubscriptions(IQueryable<UserSubscriptions> userSubscriptions);
    }
}
