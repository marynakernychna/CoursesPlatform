using CoursesPlatform.EntityFramework;
using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.ErrorMiddleware.Errors;
using CoursesPlatform.Interfaces.Queries;
using CoursesPlatform.Models.Courses;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CoursesPlatform.Services.Queries
{
    public class CourseQueries : ICoursesQueries
    {
        private AppDbContext appDbContext;

        public CourseQueries(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public IQueryable<Course> SearchTextInCourses(string searchText, IQueryable<Course> courses)
        {
            return courses.Where(u => u.Title.ToLower().Contains(searchText) ||
                                      u.Description.ToLower().Contains(searchText));
        }

        public IQueryable<Course> GetAllCourses()
        {
            return appDbContext.Courses.AsQueryable();
        }

        public IQueryable<Course> GetUserSubscriptionsById(string userId)
        {
            var subscriptions = appDbContext.UsersSubscriptions.Where(s => s.UserId == userId).AsQueryable();

            var courses = new List<Course>();

            foreach (var subscription in subscriptions)
            {
                courses.Add(GetCourseById(subscription.CourseId));
            }

            return courses.AsQueryable();
        }

        public Course GetCourseById(int courseId)
        {
            var course = appDbContext.Courses.FirstOrDefault(c => c.Id == courseId);

            if (course == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { Message = "Course not found by id!" });
            }

            return course;
        }

        public void AddCourse(Course course)
        {
            appDbContext.Courses.Add(course);

            appDbContext.SaveChanges();
        }

        public IQueryable<UserSubscriptions> GetUserSubscriptionsQueryByCourseId(int courseId)
        {
            return appDbContext.UsersSubscriptions.Where(s => s.CourseId == courseId);
        }

        public void UpdateCourse(Course course, CourseDTO newInfo)
        {
            course.Title = newInfo.Title;
            course.Description = newInfo.Description;
            course.ImageUrl = newInfo.ImageUrl;

            appDbContext.SaveChanges();
        }

        public void RemoveCourse(Course course)
        {
            appDbContext.Courses.Remove(course);
            appDbContext.SaveChanges();
        }

        public bool CheckIsSubscriptionExists(int courseId, string userId)
        {
            return appDbContext.UsersSubscriptions
                               .FirstOrDefault(s => s.CourseId == courseId && s.UserId == userId) != null;
        }

        public void AddSubscription(UserSubscriptions subscription)
        {
            appDbContext.UsersSubscriptions.Add(subscription);
            appDbContext.SaveChanges();
        }

        public bool CheckIsCourseExistsById(int courseId)
        {
            return appDbContext.Courses.FirstOrDefault(c => c.Id == courseId) != null;
        }

        public UserSubscriptions GetUserSubscription(string userId, int courseId)
        {
            return appDbContext.UsersSubscriptions.SingleOrDefault(s => s.UserId == userId && s.CourseId == courseId);
        }

        public void RemoveUserSubscription(UserSubscriptions userSubscription)
        {
            appDbContext.UsersSubscriptions.Remove(userSubscription);
            appDbContext.SaveChanges();
        }

        public IQueryable<UserSubscriptions> GetUserSubscriptions(string userId)
        {
            return appDbContext.UsersSubscriptions.Where(s => s.UserId == userId);
        }

        public void RemoveUserSubscriptions(IQueryable<UserSubscriptions> userSubscriptions)
        {
            appDbContext.UsersSubscriptions.RemoveRange(userSubscriptions);
            appDbContext.SaveChanges();
        }
    }
}
