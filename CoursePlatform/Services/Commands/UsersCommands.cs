using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.ErrorMiddleware.Errors;
using CoursesPlatform.Interfaces.Commands;
using CoursesPlatform.Interfaces.Queries;
using CoursesPlatform.Models.Courses;
using CoursesPlatform.Models.Facebook;
using CoursesPlatform.Models.Users;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoursesPlatform.Services.Commands
{
    public class UsersCommands : IUsersCommands
    {
        private readonly ICoursesCommands coursesCommands;
        private readonly ICoursesQueries coursesQueries;
        private readonly SignInManager<User> signInManager;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IUserQueries userQueries;

        public UsersCommands(ICoursesCommands coursesCommands,
                             SignInManager<User> signInManager,
                             IHttpClientFactory httpClientFactory,
                             IUserQueries userQueries,
                             ICoursesQueries coursesQueries)
        {
            this.coursesCommands = coursesCommands;
            this.httpClientFactory = httpClientFactory;
            this.signInManager = signInManager;
            this.userQueries = userQueries;
            this.coursesQueries = coursesQueries;
        }

        public async Task SignInAsync(User user, string password, bool lockoutOnFailure)
        {
            var result = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);

            if (!result.Succeeded)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "Invalid login or password !" });
            }
        }

        public User CreateNewUserModel(RegisterRequest request)
        {
            return new User
            {
                UserName = request.Email,
                Email = request.Email,
                Birthday = request.Birthday,
                Surname = request.Surname,
                Name = request.Name,
                RegisteredDate = DateTime.UtcNow
            };
        }

        public async Task<FacebookAccount> GetUserFromFacebookAsync(string facebookToken)
        {
            string facebookGraphUrl = $"https://graph.facebook.com/v4.0/me?access_token={facebookToken}&fields=email,first_name,last_name";

            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(facebookGraphUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new RestException(HttpStatusCode.InternalServerError, new { Message = "Failed to get Facebook user from token!" });
            }

            var result = await response.Content.ReadAsStringAsync();
            var facebookAccount = JsonConvert.DeserializeObject<FacebookAccount>(result);

            return facebookAccount;
        }

        public IQueryable<User> SearchTextInUsers(string searchText, IQueryable<User> users)
        {
            return users.Where(u => u.Name.ToLower().Contains(searchText) ||
                                    u.Surname.ToLower().Contains(searchText) ||
                                    u.Email.ToLower().Contains(searchText));
        }

        public IQueryable<User> SortByDirection(FilterQuery filterQuery, IQueryable<User> students)
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

        public List<StudentDTO> FormStudentsList(IQueryable<User> students)
        {
            var studentsDTOs = new List<StudentDTO>();

            foreach (var student in students)
            {
                var subscriptions = coursesCommands.GetUserSubscriptions(student.Id);

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

        public void EditUser(UserDTO newInfo, User user)
        {
            if (newInfo.Email != user.Email)
            {
                var isEmailBusy = userQueries.CheckIsUserExistsByEmail(newInfo.Email);

                if (isEmailBusy)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Message = "There is already a user with this email !" });
                }

                user.Email = newInfo.Email;
                user.UserName = newInfo.Email;
                user.NormalizedEmail = newInfo.Email.ToUpper();
                user.NormalizedUserName = newInfo.Email.ToUpper();
                user.EmailConfirmed = false;
            }

            user.Name = newInfo.Name;
            user.Surname = newInfo.Surname;
            user.Birthday = newInfo.Birthday;

            userQueries.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            DeleteUserSubscribes(user.Id);

            userQueries.RemoveUser(user);
        }

        private void DeleteUserSubscribes(string userId)
        {
            var subscriptions = coursesQueries.GetUserSubscriptions(userId);

            coursesQueries.RemoveUserSubscriptions(subscriptions);
        }

        public void EdiProfile(EditProfileRequest newInfo, User user)
        {
            if (newInfo.Email != user.Email)
            {
                var isEmailBusy = userQueries.CheckIsUserExistsByEmail(newInfo.Email);

                if (isEmailBusy)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Message = "There is already a user with this email !" });
                }

                user.Email = newInfo.Email;
                user.UserName = newInfo.Email;
                user.NormalizedEmail = newInfo.Email.ToUpper();
                user.NormalizedUserName = newInfo.Email.ToUpper();
                user.EmailConfirmed = false;
            }

            user.Name = newInfo.Name;
            user.Surname = newInfo.Surname;

            if (!string.IsNullOrEmpty(newInfo.Birthday) &&
                !string.IsNullOrWhiteSpace(newInfo.Birthday))
            {
                user.Birthday = Convert.ToDateTime(newInfo.Birthday);
            }

            userQueries.SaveChanges();
        }
    }
}
