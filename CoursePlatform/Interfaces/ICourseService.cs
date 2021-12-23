using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Models.Courses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoursesPlatform.Interfaces
{
    public interface ICourseService
    {

        #region on Page

        SubscriptionsOnPage SortAndGetUserSubscriptionsOnPage(FilterQuery request);

        CoursesOnPage SortAndGetCoursesOnPage(FilterQuery request);

        #endregion

        #region change

        void AddCourse(AddCourseRequest course);

        Task EditCourse(CourseDTO newInfo, Course oldInfo);

        void AddNewSubscription(UserSubscriptions subscription);

        void UnsubscribeFromCourse(string userId, int courseId);

        Task DeleteCourseById(int courseId);

        #endregion

        #region check

        bool CheckIfCourseExistsById(int courseId);

        bool CheckIfSubscriptionExists(int courseId, string userId);

        #endregion

        #region get by

        Course GetCourseById(int id);

        #endregion

        #region get

        List<User> GetSubscribersByCourseId(int courseId);

        List<Course> GetCourses();

        #endregion

        #region form

        UserSubscriptions FormNewSubscription(DateTime startDate, User user, Course course);

        #endregion
    }
}
