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

        #region get

        public async Task<IList<User>> GetStudents()
        {
            return await userManager.GetUsersInRoleAsync("Student");
        }

        public StudentsOnPage GetStudentsOnPage(StudentsOnPageRequest request)
        {
            var students = GetStudents().Result.AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchText) &&
                !string.IsNullOrWhiteSpace(request.SearchText) &&
                request.SearchBy.Count >= 1)
            {
                students = Search(request.SearchText, request.SearchBy, students);
            }

            int totalCount = students.Count();

            switch (request.FilterQuery.SortDirection)
            {
                case Models.Courses.FilterQuery.SortDirection_enum.ASC:
                    switch (request.FilterQuery.SortBy)
                    {
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
                        default:
                            break;
                    }
                    break;
                case Models.Courses.FilterQuery.SortDirection_enum.DESC:
                    switch (request.FilterQuery.SortBy)
                    {
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
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            return new StudentsOnPage
            {
                TotalCount = totalCount,
                Students = usersCommands.GetStudentsOnPage(request.FilterQuery, students)
            };
        }

        private IQueryable<User> Search(string searchText, List<StudentsOnPageRequest.SearchBy_enum> searchBy, IQueryable<User> users)
        {
            if (searchBy.Count == 1)
            {
                return SearchBySomething(searchText, searchBy.First(), users);
            }
            else
            {
                IQueryable<User> result = Enumerable.Empty<User>().AsQueryable();

                foreach (var item in searchBy)
                {
                    result.Concat(SearchBySomething(searchText, item, users));
                }

                return null;
            }
        }

        private static IQueryable<User> SearchBySomething(string searchText, StudentsOnPageRequest.SearchBy_enum searchBy, IQueryable<User> users)
        {
            switch (searchBy)
            {
                case StudentsOnPageRequest.SearchBy_enum.NAME:
                    {
                        return users.Where(u => u.Name.Contains(searchText));
                    }
                case StudentsOnPageRequest.SearchBy_enum.SURNAME:
                    {
                        return users.Where(u => u.Surname.Contains(searchText));
                    }
                default:
                    {
                        return null;
                    }
            }
        }

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

        //public async Task<StudentsOnPage> SearchByText(SearchStudentsRequest request)
        //{
        //    var students = await GetStudents();

        //    var searchResult = students.Where(u => u.Name.Contains(request.SearchText) ||
        //                                           u.Surname.Contains(request.SearchText) ||
        //                                           u.Email.Contains(request.SearchText))
        //                                           .ToList();

        //    int totalCount = searchResult.Count();

        //    return new StudentsOnPage
        //    {
        //        TotalCount = totalCount,
        //        Students = usersCommands.GetStudentsOnPage(request.StudentsOnPageRequest, searchResult.AsQueryable())
        //    };
        //}

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

        #endregion
    }
}