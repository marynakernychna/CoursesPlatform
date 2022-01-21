using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.ErrorMiddleware.Errors;
using CoursesPlatform.Interfaces;
using CoursesPlatform.Interfaces.Commands;
using CoursesPlatform.Interfaces.Queries;
using CoursesPlatform.Models;
using CoursesPlatform.Models.Courses;
using CoursesPlatform.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CoursesPlatform.Services
{
    public class UserService : IUserService
    {
        private readonly IUsersCommands usersCommands;
        private readonly ICoursesQueries coursesQueries;
        private readonly IGeneralCommands generalCommands;
        private readonly IUserQueries userQueries;
        private readonly IJwtUtils jwtUtils;
        private readonly IEmailService emailService;
        private readonly UserManager<User> userManager;

        public UserService(IUsersCommands usersCommands,
                           IUserQueries userQueries,
                           IGeneralCommands generalCommands,
                           IJwtUtils jwtUtils,
                           IEmailService emailService,
                           UserManager<User> userManager,
                           ICoursesQueries coursesQueries)
        {
            this.usersCommands = usersCommands;
            this.userQueries = userQueries;
            this.generalCommands = generalCommands;
            this.jwtUtils = jwtUtils;
            this.emailService = emailService;
            this.userManager = userManager;
            this.coursesQueries = coursesQueries;
        }

        public StudentsOnPage GetStudentsOnPage(GetCurrentPageRequest request)
        {
            var students = userQueries.GetStudents();

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                students = GetUsersByText(request.SearchText.ToLower(), students);
            }

            students = SortByDirection(request.FilterQuery, students);

            var totalCount = students.Count();

            students = generalCommands.GetElementsOnPage(request.FilterQuery, students);

            return new StudentsOnPage
            {
                TotalCount = totalCount,
                Students = FormStudentsList(students)
            };
        }

        private IQueryable<User> GetUsersByText(string searchText, IQueryable<User> users)
        {
            return users.Where(u => u.Name.ToLower().Contains(searchText) ||
                                    u.Surname.ToLower().Contains(searchText) ||
                                    u.Email.ToLower().Contains(searchText));
        }

        private IQueryable<User> SortByDirection(FilterQuery filterQuery, IQueryable<User> students)
        {
            switch (filterQuery.SortDirection)
            {
                case FilterQuery.SortDirection_enum.ASC:
                    {
                        students = SortByAsc(filterQuery, students);
                        break;
                    }
                case FilterQuery.SortDirection_enum.DESC:
                    {
                        students = SortByDesc(filterQuery, students);
                        break;
                    }
                default:
                    {
                        throw new RestException(HttpStatusCode.BadRequest, new { Message = "The specified <sort direction> option is missing!" });
                    }
            }

            return students;
        }

        private static IQueryable<User> SortByAsc(FilterQuery filterQuery, IQueryable<User> students)
        {
            switch (filterQuery.SortBy)
            {
                case FilterQuery.SortBy_enum.NAME:
                    {
                        students = students.OrderBy(s => s.Name);
                    }
                    break;
                case FilterQuery.SortBy_enum.SURNAME:
                    {
                        students = students.OrderBy(s => s.Surname);
                    }
                    break;
                case FilterQuery.SortBy_enum.REGISTEREDDATE:
                    {
                        students = students.OrderBy(s => s.RegisteredDate);
                    }
                    break;
                case FilterQuery.SortBy_enum.AGE:
                    {
                        students = students.OrderBy(s => s.Birthday);
                    }
                    break;
                case FilterQuery.SortBy_enum.EMAIL:
                    {
                        students = students.OrderBy(s => s.Email);
                    }
                    break;
                default:
                    {
                        throw new RestException(HttpStatusCode.BadRequest, new { Message = "The specified <sort by> option is unsupported!" });
                    }
            }

            return students;
        }

        private static IQueryable<User> SortByDesc(FilterQuery filterQuery, IQueryable<User> students)
        {
            switch (filterQuery.SortBy)
            {
                case FilterQuery.SortBy_enum.NAME:
                    {
                        students = students.OrderByDescending(s => s.Name);
                    }
                    break;
                case FilterQuery.SortBy_enum.SURNAME:
                    {
                        students = students.OrderByDescending(s => s.Surname);
                    }
                    break;
                case FilterQuery.SortBy_enum.REGISTEREDDATE:
                    {
                        students = students.OrderByDescending(s => s.RegisteredDate);
                    }
                    break;
                case FilterQuery.SortBy_enum.AGE:
                    {
                        students = students.OrderByDescending(s => s.Birthday);
                    }
                    break;
                case FilterQuery.SortBy_enum.EMAIL:
                    {
                        students = students.OrderByDescending(s => s.Email);
                    }
                    break;
                default:
                    {
                        throw new RestException(HttpStatusCode.BadRequest, new { Message = "The specified <sort by> option is unsupported!" });
                    }
            }

            return students;
        }

        private List<StudentDTO> FormStudentsList(IQueryable<User> students)
        {
            var studentsDTOs = new List<StudentDTO>();

            foreach (var student in students)
            {
                var subscriptions = GetUserSubscriptions(student.Id);

                studentsDTOs.Add(new StudentDTO
                {
                    Name = student.Name,
                    Surname = student.Surname,
                    Email = student.Email,
                    Birthday = student.Birthday,
                    IsEmailConfirmed = student.EmailConfirmed,
                    Subscriptions = subscriptions
                });
            }

            return studentsDTOs;
        }

        private List<CourseDTO> GetUserSubscriptions(string userId)
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

        public async Task EditUserAsync(EditUserRequest request, string ipAddress, HttpRequest httpRequest)
        {
            var user = userQueries.GetUserByEmail(request.CurrentUserEmail);

            if (user.Name == request.User.Name &&
                user.Surname == request.User.Surname &&
                user.Email == request.User.Email)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "The new information is the same as the previous one !" });
            }

            var oldInfo = new UserDTO
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Birthday = user.Birthday
            };

            jwtUtils.RevokeAccess(user, ipAddress);

            usersCommands.UpdateUser(request.User, user);

            await emailService.SendUserInfoChangingNotificationEmailAsync(user, oldInfo);

            if (oldInfo.Email != user.Email)
            {
                await emailService.SendConfirmationEmailAsync(user);
            }
        }

        public async Task DeleteUserAsync(StringRequest request, string ipAddress)
        {
            var user = userQueries.GetUserByEmail(request.Value);

            var userDTO = new UserDTO
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Birthday = DateTime.UtcNow
            };

            jwtUtils.RevokeAccess(user, ipAddress);

            usersCommands.DeleteUser(user);

            await emailService.SendAccountRemovalNotificationEmailAsync(userDTO);
        }

        public UserDTO GetProfileInfo(string userId)
        {
            var user = userQueries.GetUserById(userId);

            return new UserDTO
            {
                Name = user.Name,
                Surname = user.Surname,
                Birthday = user.Birthday,
                Email = user.Email
            };
        }

        public async Task EditProfileInfoAsync(EditProfileRequest request)
        {
            var user = userQueries.GetUserByEmail(request.CurrentEmail);

            if (user.Name == request.Name &&
                user.Surname == request.Surname &&
                user.Email == request.Email &&
                (string.IsNullOrEmpty(request.Birthday) ||
                string.IsNullOrWhiteSpace(request.Birthday) ||
                Convert.ToDateTime(request.Birthday) == user.Birthday))
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "The new information is the same as the previous one !" });
            }

            var oldInfo = new UserDTO
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Birthday = user.Birthday
            };

            usersCommands.UpdateProfile(request, user);

            await emailService.SendUserInfoChangingNotificationEmailAsync(user, oldInfo);

            if (oldInfo.Email != user.Email)
            {
                await emailService.SendConfirmationEmailAsync(user);
            }
        }

        public async Task ChangePasswordAsync(ChangePasswordRequest request, string userId)
        {
            var user = userQueries.GetUserById(userId);

            var result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

            if (!result.Succeeded)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "Failed to change password!" });
            }
        }
    }
}