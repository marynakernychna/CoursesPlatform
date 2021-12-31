using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Net;
using CoursesPlatform.Interfaces;
using CoursesPlatform.EntityFramework;
using CoursesPlatform.Interfaces.Commands;
using CoursesPlatform.Models.Courses;
using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.ErrorMiddleware.Errors;
using CoursesPlatform.Models.Users;

namespace CoursesPlatform.Services
{
    public class CourseService : ICourseService
    {
        private AppDbContext appDbContext;
        private readonly IUserAccessor userAccessor;
        private readonly ICoursesCommands coursesCommands;
        private readonly IHangfireService hangfireService;
        private readonly IBulkMailingService bulkMailingService;
        private readonly IUserService userService;

        public CourseService(AppDbContext appDbContext,
                             IUserAccessor userAccessor,
                             ICoursesCommands coursesCommands,
                             IHangfireService hangfireService,
                             IUserService userService,
                             IBulkMailingService bulkMailingService)
        {
            this.appDbContext = appDbContext;
            this.userAccessor = userAccessor;
            this.coursesCommands = coursesCommands;
            this.hangfireService = hangfireService;
            this.bulkMailingService = bulkMailingService;
            this.userService = userService;
        }

        #region on Page

        public CoursesOnPageResponse SortAndGetCoursesOnStudentPage(FilterQuery request)
        {
            IQueryable<Course> allCoursesQuery = appDbContext.Courses.AsQueryable();

            IQueryable<Course> sortedCourses = coursesCommands.SortCourses(request, allCoursesQuery);

            int totalCount = allCoursesQuery.Count();

            return new CoursesOnPageResponse
            {
                TotalCount = totalCount,
                Courses = coursesCommands.GetCoursesOnPage(request, sortedCourses)
            };
        }

        public SubscriptionsOnPage SortAndGetUserSubscriptionsOnPage(FilterQuery request)
        {
            string userId = userAccessor.GetCurrentUserId();

            List<CourseDTO> subscriptions = coursesCommands.GetUserSubscriptions(userId);

            IQueryable<CourseDTO> sortedSubscriptions = coursesCommands.SortSubscriptions(request, subscriptions.AsQueryable());

            int totalCount = subscriptions.Count();

            return new SubscriptionsOnPage
            {
                TotalCount = totalCount,
                Subscriptions = coursesCommands.GetUserSubscriptionsOnPage(request, sortedSubscriptions)
            };
        }

        public CoursesOnPageResponse GetCoursesOnPageAdmin(OnPageRequest request)
        {
            var courses = appDbContext.Courses.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                courses = Search(request.SearchText.ToLower(), courses);
            }

            int totalCount = courses.Count();

            courses = SortByDirection(request, courses);

            return new CoursesOnPageResponse
            {
                TotalCount = totalCount,
                Courses = coursesCommands.GetCoursesOnPage(request.FilterQuery, courses)
            };
        }

        private IQueryable<Course> Search(string searchText, IQueryable<Course> courses)
        {
            return courses.Where(u => u.Title.ToLower().Contains(searchText) ||
                                      u.Description.ToLower().Contains(searchText));
        }

        private static IQueryable<Course> SortByDirection(OnPageRequest request, IQueryable<Course> courses)
        {
            switch (request.FilterQuery.SortDirection)
            {
                case Models.Courses.FilterQuery.SortDirection_enum.ASC:
                    {
                        courses = SortByAsc(request, courses);
                        break;
                    }
                case Models.Courses.FilterQuery.SortDirection_enum.DESC:
                    {
                        courses = SortByDesc(request, courses);
                        break;
                    }
                default:
                    {
                        throw new RestException(HttpStatusCode.BadRequest, new { Message = "The specified <sort direction> option is missing!" });
                    }
            }

            return courses;
        }

        private static IQueryable<Course> SortByAsc(OnPageRequest request, IQueryable<Course> courses)
        {
            switch (request.FilterQuery.SortBy)
            {
                case FilterQuery.SortBy_enum.TITLE:
                    {
                        courses = courses.OrderBy(s => s.Title);
                    }
                    break;
                case FilterQuery.SortBy_enum.DATE:
                    {
                        courses = courses.OrderBy(s => s.CreateDate);
                    }
                    break;
                default:
                    {
                        throw new RestException(HttpStatusCode.BadRequest, new { Message = "The specified <sort by> option is missing!" });
                    }
            }

            return courses;
        }

        private static IQueryable<Course> SortByDesc(OnPageRequest request, IQueryable<Course> courses)
        {
            switch (request.FilterQuery.SortBy)
            {
                case FilterQuery.SortBy_enum.TITLE:
                    {
                        courses = courses.OrderByDescending(s => s.Title);
                    }
                    break;
                case FilterQuery.SortBy_enum.DATE:
                    {
                        courses = courses.OrderByDescending(s => s.CreateDate);
                    }
                    break;
                default:
                    {
                        throw new RestException(HttpStatusCode.BadRequest, new { Message = "The specified <sort by> option is missing!" });
                    }
            }

            return courses;
        }

        #endregion

        #region change

        public void AddCourse(AddCourseRequest course)
        {
            Course newCourse = new Course()
            {
                Title = course.Title,
                Description = course.Description,
                ImageUrl = course.ImageUrl,
                CreateDate = DateTime.UtcNow
            };

            appDbContext.Courses.Add(newCourse);

            appDbContext.SaveChanges();
        }

        public async Task EditCourse(CourseDTO newInfo, Course oldInfo)
        {
            List<User> courseSubscribers = GetSubscribersByCourseId(newInfo.Id);

            string oldTitle = oldInfo.Title;
            string oldDescription = oldInfo.Description;

            Course course = GetCourseById(newInfo.Id);

            course.Title = newInfo.Title;
            course.Description = newInfo.Description;
            course.ImageUrl = newInfo.ImageUrl;

            appDbContext.SaveChanges();

            if (courseSubscribers.Count > 0)
            {
                await bulkMailingService.SendCourseEditingNotificationEmails(courseSubscribers, newInfo, oldTitle, oldDescription);
            }
        }

        public void AddNewSubscription(UserSubscriptions subscription)
        {
            appDbContext.UsersSubscriptions.Add(subscription);
            appDbContext.SaveChanges();

            string userEmail = userService.GetUserEmailById(subscription.UserId);

            string courseTitle = GetCourseById(subscription.CourseId).Title;

            hangfireService.SetCourseStartNotifications(subscription, userEmail, courseTitle);
        }

        public void UnsubscribeFromCourse(string userId, int courseId)
        {
            var subscriptionId = GetUserSubscription(userId, courseId).Id;

            hangfireService.DeleteCourseStartNotifications(subscriptionId);

            coursesCommands.UnsubscribeFromCourse(userId, courseId);
        }

        public async Task DeleteCourseById(int courseId)
        {
            List<User> courseSubscribers = GetSubscribersByCourseId(courseId);

            Course course = GetCourseById(courseId);

            if (courseSubscribers.Count > 0)
            {
                await bulkMailingService.SendCourseRemovalNotificationEmails(courseSubscribers, course.Title);
            }

            appDbContext.Courses.Remove(course);
            appDbContext.SaveChanges();
        }

        #endregion

        #region check

        public bool CheckIsCourseExistsById(int courseId)
        {
            return appDbContext.Courses.FirstOrDefault(c => c.Id == courseId) != null;
        }

        public bool CheckIsSubscriptionExists(int courseId, string userId)
        {
            return appDbContext.UsersSubscriptions
                               .FirstOrDefault(s => s.CourseId == courseId && s.UserId == userId) != null;
        }

        #endregion

        #region get by

        public Course GetCourseById(int id)
        {
            var course = appDbContext.Courses.FirstOrDefault(c => c.Id == id);

            if (course == null)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "Course not found !" });
            }

            return course;
        }

        #endregion

        #region get

        public List<User> GetSubscribersByCourseId(int courseId)
        {
            var subscriptions = coursesCommands.GetUserSubscriptionsQueryByCourseId(courseId);

            List<User> users = new List<User>();

            foreach (var item in subscriptions)
            {
                var user = userService.GetUserById(item.UserId);

                users.Add(user);
            }

            return users;
        }

        public List<Course> GetCoursesOnPageAdmin()
        {
            return appDbContext.Courses.ToList();
        }

        private UserSubscriptions GetUserSubscription(string userId, int courseId)
        {
            return appDbContext.UsersSubscriptions.SingleOrDefault(s => s.UserId == userId && s.CourseId == courseId);
        }

        #endregion

        #region form

        public UserSubscriptions CreateNewSubscriptionModel(DateTime startDate, User user, Course course)
        {
            return new UserSubscriptions()
            {
                CourseId = course.Id,
                Course = course,
                UserId = user.Id,
                User = user,
                StartDate = startDate
            };
        }

        #endregion

        #region not labeled

        #endregion
    }
}
