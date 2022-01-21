using System.Linq;
using System;
using System.Threading.Tasks;
using CoursesPlatform.Interfaces;
using CoursesPlatform.Interfaces.Commands;
using CoursesPlatform.Models.Courses;
using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Models.Users;
using CoursesPlatform.Interfaces.Queries;
using CoursesPlatform.ErrorMiddleware.Errors;
using System.Net;
using System.Collections.Generic;

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
                courses = coursesQueries.GetCoursesByText(request.SearchText.ToLower(), courses);
            }

            courses = SortCoursesByDirection(request.FilterQuery, courses);

            return new CoursesOnPageResponse
            {
                TotalCount = courses.Count(),
                Courses = generalCommands.GetElementsOnPage(request.FilterQuery, courses).ToList()
            };
        }

        private IQueryable<Course> SortCoursesByDirection(FilterQuery request, IQueryable<Course> courses)
        {
            switch (request.SortDirection)
            {
                case FilterQuery.SortDirection_enum.ASC:
                    {
                        courses = SortCoursesByAsc(request, courses);
                        break;
                    }
                case FilterQuery.SortDirection_enum.DESC:
                    {
                        courses = SortCoursesByDesc(request, courses);
                        break;
                    }
                default:
                    {
                        throw new RestException(HttpStatusCode.BadRequest, new { Message = "The specified <sort direction> option is missing!" });
                    }
            }

            return courses;
        }

        private IQueryable<Course> SortCoursesByAsc(FilterQuery request, IQueryable<Course> courses)
        {
            switch (request.SortBy)
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
                        throw new RestException(HttpStatusCode.BadRequest, new { Message = "The specified <sort by> option is unsupported!" });
                    }
            }

            return courses;
        }

        private IQueryable<Course> SortCoursesByDesc(FilterQuery request, IQueryable<Course> courses)
        {
            switch (request.SortBy)
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
                        throw new RestException(HttpStatusCode.BadRequest, new { Message = "The specified <sort by> option is unsupported!" });
                    }
            }

            return courses;
        }

        public CoursesOnPageResponse SortAndGetCoursesOnStudentPage(FilterQuery request)
        {
            var courses = coursesQueries.GetAllCourses();

            courses = SortCoursesByDirection(request, courses);

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

            subscriptions = SortCoursesByDirection(request, subscriptions);

            var courses = FormCoursesDTOsFromCourses(subscriptions);

            var totalCount = courses.Count();

            return new SubscriptionsOnPage
            {
                TotalCount = totalCount,
                Subscriptions = generalCommands.GetElementsOnPage(request, courses).ToList()
            };
        }

        private IQueryable<CourseDTO> FormCoursesDTOsFromCourses(IQueryable<Course> courses)
        {
            var coursesDTOs = new List<CourseDTO>();

            foreach (var course in courses)
            {
                coursesDTOs.Add(new CourseDTO
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                    ImageUrl = course.ImageUrl
                });
            }

            return coursesDTOs.AsQueryable();
        }

        public void AddCourse(AddCourseRequest course)
        {
            coursesCommands.CreateCourse(new Course
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
            var courseSubscribers = GetSubscribersByCourseId(newInfo.Id);

            var oldTitle = oldInfo.Title;
            var oldDescription = oldInfo.Description;

            var course = coursesQueries.GetCourseById(newInfo.Id);

            coursesCommands.UpdateCourse(course, newInfo);

            if (courseSubscribers.Count > 0)
            {
                await bulkMailingService.SendCourseEditingNotificationEmailsAsync(courseSubscribers, newInfo, oldTitle, oldDescription);
            }
        }

        private List<User> GetSubscribersByCourseId(int courseId)
        {
            var subscriptions = coursesQueries.GetUserSubscriptionsQueryByCourseId(courseId);

            var users = new List<User>();

            foreach (var subscription in subscriptions)
            {
                users.Add(subscription.User);
            }

            return users;
        }

        public async Task DeleteCourseByIdAsync(int courseId)
        {
            var courseSubscribers = GetSubscribersByCourseId(courseId);

            var course = coursesQueries.GetCourseById(courseId);

            if (courseSubscribers.Count > 0)
            {
                await bulkMailingService.SendCourseRemovalNotificationEmailsAsync(courseSubscribers, course.Title);
            }

            coursesCommands.DeleteCourse(course);
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
                UserId = user.Id,
                StartDate = startDate
            };
        }

        public void AddNewSubscription(UserSubscriptions subscription)
        {
            coursesCommands.CreateSubscription(subscription);

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

            coursesCommands.DeleteUserSubscription(subscription);
        }
    }
}
