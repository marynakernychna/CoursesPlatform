using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Models.Courses;
using CoursesPlatform.Models.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoursesPlatform.Interfaces
{
    public interface ICourseService
    {

        #region on Page

        SubscriptionsOnPage SortAndGetUserSubscriptionsOnPage(FilterQuery request);

        CoursesOnPageResponse SortAndGetCoursesOnStudentPage(FilterQuery request);

        CoursesOnPageResponse GetCoursesOnPageAdmin(OnPageRequest request);

        #endregion

        #region change

        void AddCourse(AddCourseRequest course);

        Task EditCourse(CourseDTO newInfo, Course oldInfo);

        void AddNewSubscription(UserSubscriptions subscription);

        void UnsubscribeFromCourse(string userId, int courseId);

        Task DeleteCourseById(int courseId);

        #endregion

        #region check

        bool CheckIsCourseExistsById(int courseId);

        bool CheckIsSubscriptionExists(int courseId, string userId);

        #endregion

        #region get by

        Course GetCourseById(int id);

        #endregion

        #region get

        List<User> GetSubscribersByCourseId(int courseId);

        #endregion

        #region form

        UserSubscriptions CreateNewSubscriptionModel(DateTime startDate, User user, Course course);

        #endregion
    }
}
