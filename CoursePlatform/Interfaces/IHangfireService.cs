using CoursesPlatform.EntityFramework.Models;

namespace CoursesPlatform.Interfaces
{
    public interface IHangfireService
    {
        void DeleteCourseStartNotifications(int subscriptionId);

        void SetCourseStartNotifications(UserSubscriptions subscription, string userEmail, string courseTitle);
    }
}
