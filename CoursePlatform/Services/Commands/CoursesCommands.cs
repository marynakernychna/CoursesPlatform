using CoursesPlatform.EntityFramework;
using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.ErrorMiddleware.Errors;
using CoursesPlatform.Interfaces;
using CoursesPlatform.Interfaces.Commands;
using CoursesPlatform.Models.Courses;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CoursesPlatform.Services.Commands
{
    public class CoursesCommands : ICoursesCommands
    {
        private AppDbContext appDbContext;
        private readonly IHangfireService hangfireService;

        public CoursesCommands(AppDbContext appDbContext,
                               IHangfireService hangfireService)
        {
            this.appDbContext = appDbContext;
            this.hangfireService = hangfireService;
        }

        #region on Page

        public IQueryable<Course> SortCourses(FilterQuery request, IQueryable<Course> courses)
        {
            switch (request.SortDirection)
            {
                case FilterQuery.SortDirection_enum.ASC:
                    {
                        return SortCoursesASCBySomething(request.SortBy, courses);
                    }
                case FilterQuery.SortDirection_enum.DESC:
                    {
                        return SortCoursesDESCBySomething(request.SortBy, courses);
                    }
                default:
                    {
                        throw new RestException(HttpStatusCode.BadRequest, new { Message = "Invalid sort direction type !" });
                    }
            };
        }

        public List<Course> GetCoursesOnPage(FilterQuery request, IQueryable<Course> courses)
        {
            return courses.Skip((request.PageNumber - 1) * request.ElementsOnPage)
                          .Take(request.ElementsOnPage)
                          .ToList();
        }

        public IQueryable<CourseDTO> SortSubscriptions(FilterQuery request, IQueryable<CourseDTO> courses)
        {
            switch (request.SortDirection)
            {
                case FilterQuery.SortDirection_enum.ASC:
                    {
                        return SortBySomethingASCSubs(request.SortBy, courses);
                    }
                case FilterQuery.SortDirection_enum.DESC:
                    {
                        return SortBySomethingDESCSubs(request.SortBy, courses);
                    }
                default:
                    {
                        throw new RestException(HttpStatusCode.BadRequest, new { Message = "Invalid sort direction !" });
                    }
            };
        }

        public List<CourseDTO> GetUserSubscriptionsOnPage(FilterQuery request, IQueryable<CourseDTO> courses)
        {
            return courses.Skip((request.PageNumber - 1) * request.ElementsOnPage)
                          .Take(request.ElementsOnPage)
                          .ToList();
        }

        #endregion

        #region change

        public void UnsubscribeFromCourse(string userId, int courseId)
        {
            var subscription = GetUserSubscription(courseId, userId);

            appDbContext.UsersSubscriptions.Remove(subscription);
            appDbContext.SaveChanges();
        }


        #endregion

        #region get

        public List<UserSubscriptions> GetUserSubscriptionsQueryByCourseId(int courseId)
        {
            return appDbContext.UsersSubscriptions.Where(s => s.CourseId == courseId).ToList();
        }

        public UserSubscriptions GetUserSubscription(int courseId, string userId)
        {
            var subscription = appDbContext.UsersSubscriptions.FirstOrDefault(s => s.CourseId == courseId && s.UserId == userId);

            if (subscription == null)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "Subscription not found !" });
            }

            return subscription;
        }

        public List<CourseDTO> GetUserSubscriptions(string userId)
        {
            var subscriptions = appDbContext.UsersSubscriptions.Where(s => s.UserId == userId)
                                                               .AsQueryable();

            List<CourseDTO> courses = new List<CourseDTO>();

            foreach (var subscription in subscriptions)
            {
                Course course = appDbContext.Courses.FirstOrDefault(c => c.Id == subscription.CourseId);

                if (course == null)
                {
                    throw new InternalServerError();
                }

                courses.Add(new CourseDTO
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                    ImageUrl = course.ImageUrl
                });
            }

            return courses;
        }

        #endregion

        #region private

        private static IQueryable<CourseDTO> SortBySomethingASCSubs(FilterQuery.SortBy_enum sortBy, IQueryable<CourseDTO> courses)
        {
            switch (sortBy)
            {
                case FilterQuery.SortBy_enum.DATE:
                    {
                        return courses = courses.OrderBy(c => c.Description);
                    }
                case FilterQuery.SortBy_enum.TITLE:
                    {
                        return courses = courses.OrderBy(c => c.Title);
                    }
                default:
                    {
                        throw new RestException(HttpStatusCode.BadRequest, new { Message = "Invalid sort by !" });
                    }
            }
        }

        private static IQueryable<CourseDTO> SortBySomethingDESCSubs(FilterQuery.SortBy_enum sortBy, IQueryable<CourseDTO> courses)
        {
            switch (sortBy)
            {
                case FilterQuery.SortBy_enum.TITLE:
                    {
                        return courses = courses.OrderByDescending(c => c.Title);
                    }
                case FilterQuery.SortBy_enum.DATE:
                    {
                        return courses = courses.OrderByDescending(c => c.Description);
                    }
                default:
                    {
                        throw new RestException(HttpStatusCode.BadRequest, new { Message = "Invalid sort by !" });
                    }
            }
        }

        private static IQueryable<Course> SortCoursesASCBySomething(FilterQuery.SortBy_enum sortBy, IQueryable<Course> courses)
        {
            switch (sortBy)
            {
                case FilterQuery.SortBy_enum.DATE:
                    {
                        return courses = courses.OrderBy(c => c.CreateDate);
                    }
                case FilterQuery.SortBy_enum.TITLE:
                    {
                        return courses = courses.OrderBy(c => c.Title);
                    }
                default:
                    {
                        throw new RestException(HttpStatusCode.BadRequest, new { Message = "Invalid sort by type !" });
                    }
            }
        }

        private static IQueryable<Course> SortCoursesDESCBySomething(FilterQuery.SortBy_enum sortBy, IQueryable<Course> courses)
        {
            switch (sortBy)
            {
                case FilterQuery.SortBy_enum.TITLE:
                    {
                        return courses = courses.OrderByDescending(c => c.Title);
                    }
                case FilterQuery.SortBy_enum.DATE:
                    {
                        return courses = courses.OrderByDescending(c => c.CreateDate);
                    }
                default:
                    {
                        throw new RestException(HttpStatusCode.BadRequest, new { Message = "Invalid sort by type !" });
                    }
            }
        }

        #endregion

    }
}
