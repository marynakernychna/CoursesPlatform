using CoursesPlatform.EntityFramework;
using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.ErrorMiddleware.Errors;
using CoursesPlatform.Interfaces.Queries;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CoursesPlatform.Services.Queries
{
    public class CourseQueries : ICoursesQueries
    {
        private readonly AppDbContext appDbContext;

        public CourseQueries(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public IQueryable<Course> GetAllCourses()
        {
            return appDbContext.Courses.AsQueryable().AsNoTracking();
        }

        public IQueryable<Course> GetCoursesByText(string searchText, IQueryable<Course> courses)
        {
            return courses.Where(u => u.Title.ToLower().Contains(searchText) ||
                                      u.Description.ToLower().Contains(searchText))
                          .AsNoTracking();
        }

        public IQueryable<Course> GetUserSubscriptionsById(string userId)
        {
            var subscriptions = appDbContext.UsersSubscriptions.Where(s => s.UserId == userId)
                                                               .Include(s => s.Course)
                                                               .AsQueryable()
                                                               .AsNoTracking();

            var courses = new List<Course>();

            foreach (var subscription in subscriptions)
            {
                courses.Add(subscription.Course);
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

        public IQueryable<UserSubscriptions> GetUserSubscriptionsQueryByCourseId(int courseId)
        {
            return appDbContext.UsersSubscriptions.Where(s => s.CourseId == courseId)
                                                  .Include(s => s.User)
                                                  .AsNoTracking();
        }

        public bool CheckIsSubscriptionExists(int courseId, string userId)
        {
            return appDbContext.UsersSubscriptions.FirstOrDefault(s => s.CourseId == courseId && s.UserId == userId) != null;
        }

        public bool CheckIsCourseExistsById(int courseId)
        {
            return appDbContext.Courses.FirstOrDefault(c => c.Id == courseId) != null;
        }

        public UserSubscriptions GetUserSubscription(string userId, int courseId)
        {
            var subscription = appDbContext.UsersSubscriptions.SingleOrDefault(s => s.UserId == userId && s.CourseId == courseId);

            if (subscription == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { Message = "Subscription not found by id!" });
            }

            return subscription;
        }

        public IQueryable<UserSubscriptions> GetUserSubscriptions(string userId)
        {
            return appDbContext.UsersSubscriptions.Where(s => s.UserId == userId)
                                                  .AsNoTracking();
        }
    }
}
