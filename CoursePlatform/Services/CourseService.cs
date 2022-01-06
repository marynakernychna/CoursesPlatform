using System.Linq;
using System;
using System.Threading.Tasks;
using CoursesPlatform.Interfaces;
using CoursesPlatform.Interfaces.Commands;
using CoursesPlatform.Models.Courses;
using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Models.Users;
using CoursesPlatform.Interfaces.Queries;

namespace CoursesPlatform.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUserAccessor userAccessor;

        private readonly ICoursesCommands coursesCommands;
        private readonly IGeneralCommands generalCommands;

        private readonly ICoursesQueries coursesQueries;
        private readonly IUserQueries userQueries;

        private readonly IHangfireService hangfireService;
        private readonly IBulkMailingService bulkMailingService;

        public CourseService(IUserAccessor userAccessor,
                             ICoursesCommands coursesCommands,
                             IHangfireService hangfireService,
                             IBulkMailingService bulkMailingService,
                             ICoursesQueries coursesQueries,
                             IGeneralCommands generalCommands,
                             IUserQueries userQueries)
        {
            this.userAccessor = userAccessor;
            this.coursesCommands = coursesCommands;
            this.generalCommands = generalCommands;
            this.coursesQueries = coursesQueries;
            this.userQueries = userQueries;
            this.hangfireService = hangfireService;
            this.bulkMailingService = bulkMailingService;
        }

        public CoursesOnPageResponse GetCoursesOnAdminPage(GetCurrentPageRequest request)
        {
            var courses = coursesQueries.GetAllCourses();

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                courses = coursesQueries.SearchTextInCourses(request.SearchText.ToLower(), courses);
            }

            courses = coursesCommands.SortCoursesByDirection(request.FilterQuery, courses);

            return new CoursesOnPageResponse
            {
                TotalCount = courses.Count(),
                Courses = generalCommands.GetElementsOnPage(request.FilterQuery, courses).ToList()
            };
        }

        public CoursesOnPageResponse SortAndGetCoursesOnStudentPage(FilterQuery request)
        {
            var courses = coursesQueries.GetAllCourses();

            courses = coursesCommands.SortCoursesByDirection(request, courses);

            return new CoursesOnPageResponse
            {
                TotalCount = courses.Count(),
                Courses = generalCommands.GetElementsOnPage(request, courses).ToList()
            };
        }

        public SubscriptionsOnPage SortAndGetUserSubscriptionsOnPage(FilterQuery request)
        {
            var userId = userAccessor.GetCurrentUserId();

            var subscriptions = coursesQueries.GetUserSubscriptionsById(userId);

            subscriptions = coursesCommands.SortCoursesByDirection(request, subscriptions);

            var courses = coursesCommands.FormCoursesDTOsFromCourses(subscriptions);

            int totalCount = courses.Count();

            return new SubscriptionsOnPage
            {
                TotalCount = totalCount,
                Subscriptions = generalCommands.GetElementsOnPage(request, courses).ToList()
            };
        }

        public void AddCourse(AddCourseRequest course)
        {
            coursesQueries.AddCourse(new Course
            {
                Title = course.Title,
                Description = course.Description,
                ImageUrl = course.ImageUrl,
                CreateDate = DateTime.UtcNow
            });
        }

        public Course GetCourseById(int id)
        {
            return coursesQueries.GetCourseById(id);
        }

        public bool CheckIsOldUserInfoIsEqualToOld(Course oldInfo, CourseDTO newInfo)
        {
            return oldInfo.Title == newInfo.Title &&
                   oldInfo.Description == newInfo.Description &&
                   oldInfo.ImageUrl == newInfo.ImageUrl;
        }

        public async Task EditCourseAsync(CourseDTO newInfo, Course oldInfo)
        {
            var courseSubscribers = coursesCommands.GetSubscribersByCourseId(newInfo.Id);

            var oldTitle = oldInfo.Title;
            var oldDescription = oldInfo.Description;

            var course = coursesQueries.GetCourseById(newInfo.Id);

            coursesQueries.UpdateCourse(course, newInfo);

            if (courseSubscribers.Count > 0)
            {
                await bulkMailingService.SendCourseEditingNotificationEmailsAsync(courseSubscribers, newInfo, oldTitle, oldDescription);
            }
        }

        public async Task DeleteCourseByIdAsync(int courseId)
        {
            var courseSubscribers = coursesCommands.GetSubscribersByCourseId(courseId);

            var course = coursesQueries.GetCourseById(courseId);

            if (courseSubscribers.Count > 0)
            {
                await bulkMailingService.SendCourseRemovalNotificationEmailsAsync(courseSubscribers, course.Title);
            }

            coursesQueries.RemoveCourse(course);
        }

        public bool CheckIsSubscriptionExists(int courseId, string userId)
        {
            return coursesQueries.CheckIsSubscriptionExists(courseId, userId);
        }

        public UserSubscriptions CreateNewSubscriptionModel(string userId, int courseId, DateTime startDate)
        {
            var user = userQueries.GetUserById(userId);
            var course = coursesQueries.GetCourseById(courseId);

            return new UserSubscriptions()
            {
                CourseId = course.Id,
                Course = course,
                UserId = user.Id,
                User = user,
                StartDate = startDate
            };
        }

        public void AddNewSubscription(UserSubscriptions subscription)
        {
            coursesQueries.AddSubscription(subscription);

            var userEmail = userQueries.GetUserEmailById(subscription.UserId);

            var courseTitle = coursesQueries.GetCourseById(subscription.CourseId).Title;

            hangfireService.SetCourseStartNotifications(subscription, userEmail, courseTitle);
        }

        public bool CheckIsCourseExistsById(int courseId)
        {
            return coursesQueries.CheckIsCourseExistsById(courseId);
        }

        public void UnsubscribeFromCourse(string userId, int courseId)
        {
            var subscription = coursesQueries.GetUserSubscription(userId, courseId);

            hangfireService.DeleteCourseStartNotifications(subscription.Id);

            coursesQueries.RemoveUserSubscription(subscription);
        }
    }
}
