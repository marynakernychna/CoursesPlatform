using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Models.Courses;
using CoursesPlatform.Models.Users;
using System;
using System.Threading.Tasks;

namespace CoursesPlatform.Interfaces
{
    public interface ICourseService
    {
        CoursesOnPageResponse GetCoursesOnAdminPage(GetCurrentPageRequest request);

        CoursesOnPageResponse SortAndGetCoursesOnStudentPage(FilterQuery request);

        SubscriptionsOnPage SortAndGetUserSubscriptionsOnPage(FilterQuery request);

        void AddCourse(AddCourseRequest course);

        Course GetCourseById(int id);

        bool CheckIsOldUserInfoIsEqualToOld(Course oldInfo, CourseDTO newInfo);

        Task EditCourseAsync(CourseDTO newInfo, Course oldInfo);

        Task DeleteCourseByIdAsync(int courseId);

        bool CheckIsSubscriptionExists(int courseId, string userId);

        UserSubscriptions CreateNewSubscriptionModel(string userId, int courseId, DateTime startDate);

        void AddNewSubscription(UserSubscriptions subscription);

        bool CheckIsCourseExistsById(int courseId);

        void UnsubscribeFromCourse(string userId, int courseId);
    }
}
