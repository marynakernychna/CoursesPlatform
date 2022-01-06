using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.ErrorMiddleware.Errors;
using CoursesPlatform.Interfaces.Commands;
using CoursesPlatform.Interfaces.Queries;
using CoursesPlatform.Models.Courses;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CoursesPlatform.Services.Commands
{
    public class CoursesCommands : ICoursesCommands
    {
        private readonly ICoursesQueries coursesQueries;
        private readonly IUserQueries userQueries;

        public CoursesCommands(ICoursesQueries coursesQueries,
                               IUserQueries userQueries)
        {
            this.coursesQueries = coursesQueries;
            this.userQueries = userQueries;
        }

        public IQueryable<Course> SortCoursesByDirection(FilterQuery request, IQueryable<Course> courses)
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

        public IQueryable<CourseDTO> FormCoursesDTOsFromCourses(IQueryable<Course> courses)
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

        public List<User> GetSubscribersByCourseId(int courseId)
        {
            var subscriptions = coursesQueries.GetUserSubscriptionsQueryByCourseId(courseId);

            var users = new List<User>();

            foreach (var subscription in subscriptions)
            {
                users.Add(userQueries.GetUserById(subscription.UserId));
            }

            return users;
        }

        public List<CourseDTO> GetUserSubscriptions(string userId)
        {
            var subscriptions = coursesQueries.GetUserSubscriptionsById(userId);

            var coursesDTOs = new List<CourseDTO>();

            foreach (var course in subscriptions)
            {
                coursesDTOs.Add(new CourseDTO
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                    ImageUrl = course.ImageUrl
                });
            }

            return coursesDTOs;
        }
    }
}
