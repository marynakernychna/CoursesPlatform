using Microsoft.AspNetCore.Identity;
using CoursesPlatform.ErrorMiddleware.Errors;
using CoursesPlatform.Interfaces;
using CoursesPlatform.Interfaces.Commands;
using CoursesPlatform.Models.Users;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoursesPlatform.EntityFramework;
using CoursesPlatform.EntityFramework.Models;
using System;

namespace CoursesPlatform.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext appDbContext;
        private readonly UserManager<User> userManager;
        private readonly IUsersCommands usersCommands;

        public UserService(AppDbContext appDbContext,
                           UserManager<User> userManager,
                           IUsersCommands usersCommands)
        {
            this.appDbContext = appDbContext;
            this.userManager = userManager;
            this.usersCommands = usersCommands;
        }

        #region on page

        public StudentsOnPage GetStudentsOnPage(OnPageRequest request)
        {
            var students = GetStudents();

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                students = Search(request.SearchText.ToLower(), students);
            }

            int totalCount = students.Count();

            students = SortByDirection(request, students);

            return new StudentsOnPage
            {
                TotalCount = totalCount,
                Students = usersCommands.GetStudentsOnPage(request.FilterQuery, students)
            };
        }

        private IQueryable<User> GetStudents()
        {
            var roleId = appDbContext.Roles.FirstOrDefault(r => r.Name == "Student").Id;

            if (string.IsNullOrEmpty(roleId))
            {
                throw new RestException(HttpStatusCode.NotFound, new { Message = "Role <Student> not found!" });
            }

            var usersInRoleId = appDbContext.UserRoles.Where(u => u.RoleId == roleId);

            return appDbContext.Users.Where(u => usersInRoleId.Any(r => r.UserId == u.Id));
        }

        private IQueryable<User> Search(string searchText, IQueryable<User> users)
        {
            return users.Where(u => u.Name.ToLower().Contains(searchText) ||
                                    u.Surname.ToLower().Contains(searchText) ||
                                    u.Email.ToLower().Contains(searchText));
        }

        private static IQueryable<User> SortByDirection(OnPageRequest request, IQueryable<User> students)
        {
            switch (request.FilterQuery.SortDirection)
            {
                case Models.Courses.FilterQuery.SortDirection_enum.ASC:
                    {
                        students = SortByAsc(request, students);
                        break;
                    }
                case Models.Courses.FilterQuery.SortDirection_enum.DESC:
                    {
                        students = SortByDesc(request, students);
                        break;
                    }
                default:
                    {
                        throw new RestException(HttpStatusCode.BadRequest, new { Message = "The specified <sort direction> option is missing!" });
                    }
            }

            return students;
        }

        private static IQueryable<User> SortByAsc(OnPageRequest request, IQueryable<User> students)
        {
            switch (request.FilterQuery.SortBy)
            {
                case Models.Courses.FilterQuery.SortBy_enum.NAME:
                    {
                        students = students.OrderBy(s => s.Name);
                    }
                    break;
                case Models.Courses.FilterQuery.SortBy_enum.SURNAME:
                    {
                        students = students.OrderBy(s => s.Surname);
                    }
                    break;
                case Models.Courses.FilterQuery.SortBy_enum.REGISTEREDDATE:
                    {
                        students = students.OrderBy(s => s.RegisteredDate);
                    }
                    break;
                case Models.Courses.FilterQuery.SortBy_enum.AGE:
                    {
                        students = students.OrderBy(s => s.Birthday);
                    }
                    break;
                case Models.Courses.FilterQuery.SortBy_enum.EMAIL:
                    {
                        students = students.OrderBy(s => s.Email);
                    }
                    break;
                default:
                    {
                        throw new RestException(HttpStatusCode.BadRequest, new { Message = "The specified <sort by> option is missing!" });
                    }
            }

            return students;
        }

        private static IQueryable<User> SortByDesc(OnPageRequest request, IQueryable<User> students)
        {
            switch (request.FilterQuery.SortBy)
            {
                case Models.Courses.FilterQuery.SortBy_enum.NAME:
                    {
                        students = students.OrderByDescending(s => s.Name);
                    }
                    break;
                case Models.Courses.FilterQuery.SortBy_enum.SURNAME:
                    {
                        students = students.OrderByDescending(s => s.Surname);
                    }
                    break;
                case Models.Courses.FilterQuery.SortBy_enum.REGISTEREDDATE:
                    {
                        students = students.OrderByDescending(s => s.RegisteredDate);
                    }
                    break;
                case Models.Courses.FilterQuery.SortBy_enum.AGE:
                    {
                        students = students.OrderByDescending(s => s.Birthday);
                    }
                    break;
                case Models.Courses.FilterQuery.SortBy_enum.EMAIL:
                    {
                        students = students.OrderByDescending(s => s.Email);
                    }
                    break;
                default:
                    {
                        throw new RestException(HttpStatusCode.BadRequest, new { Message = "The specified <sort by> option is missing!" });
                    }
            }

            return students;
        }

        #endregion

        #region get

        public string GetUserIdByEmail(string email)
        {
            return GetUserByEmail(email).Id;
        }

        public string GetUserEmailById(string userId)
        {
            return GetUserById(userId).Email;
        }

        public User GetUserById(string id)
        {
            var user = appDbContext.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "User not found !" });
            }

            return user;
        }

        public User GetUserByEmail(string email)
        {
            var user = appDbContext.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "User not found by email !" });
            }

            return user;
        }

        public UserDTO GetProfileInfo(string userId)
        {
            User user = GetUserById(userId);

            return new UserDTO
            {
                Name = user.Name,
                Surname = user.Surname,
                Birthday = user.Birthday,
                Email = user.Email
            };
        }

        #endregion

        #region change

        public void EditUser(UserDTO newInfo, User user)
        {
            if (newInfo.Email != user.Email)
            {
                bool isEmailBusy = CheckIsUserExistsByEmail(newInfo.Email);

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

            appDbContext.SaveChanges();
        }

        public void EdiProfile(EditProfileRequest newInfo, User user)
        {
            if (newInfo.Email != user.Email)
            {
                bool isEmailBusy = CheckIsUserExistsByEmail(newInfo.Email);

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

            appDbContext.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            usersCommands.DeleteUserSubscribes(user.Id);

            appDbContext.Users.Remove(user);

            appDbContext.SaveChanges();
        }

        #endregion

        #region check

        public bool CheckIsUserExistsByEmail(string email)
        {
            return appDbContext.Users.FirstOrDefault(u => u.Email == email) != null;
        }

        #endregion

        #region not labeled


        #endregion
    }
}