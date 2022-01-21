using CoursesPlatform.EntityFramework;
using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Interfaces.Commands;
using CoursesPlatform.Models.Courses;
using System.Linq;

namespace CoursesPlatform.Services.Commands
{
    public class CoursesCommands : ICoursesCommands
    {
        private AppDbContext appDbContext;

        public CoursesCommands(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public void CreateCourse(Course course)
        {
            appDbContext.Courses.Add(course);

            appDbContext.SaveChanges();
        }

        public void UpdateCourse(Course course, CourseDTO newInfo)
        {
            course.Title = newInfo.Title;
            course.Description = newInfo.Description;
            course.ImageUrl = newInfo.ImageUrl;

            appDbContext.SaveChanges();
        }

        public void DeleteCourse(Course course)
        {
            appDbContext.Courses.Remove(course);
            appDbContext.SaveChanges();
        }

        public void CreateSubscription(UserSubscriptions subscription)
        {
            appDbContext.UsersSubscriptions.Add(subscription);
            appDbContext.SaveChanges();
        }

        public void DeleteUserSubscription(UserSubscriptions userSubscription)
        {
            appDbContext.UsersSubscriptions.Remove(userSubscription);
            appDbContext.SaveChanges();
        }

        public void DeleteUserSubscriptions(IQueryable<UserSubscriptions> userSubscriptions)
        {
            appDbContext.UsersSubscriptions.RemoveRange(userSubscriptions);
            appDbContext.SaveChanges();
        }
    }
}
