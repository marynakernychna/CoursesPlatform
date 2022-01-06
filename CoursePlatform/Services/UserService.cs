using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.ErrorMiddleware.Errors;
using CoursesPlatform.Interfaces;
using CoursesPlatform.Interfaces.Commands;
using CoursesPlatform.Interfaces.Queries;
using CoursesPlatform.Models;
using CoursesPlatform.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CoursesPlatform.Services
{
    public class UserService : IUserService
    {
        private readonly IUsersCommands usersCommands;
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
                           UserManager<User> userManager)
        {
            this.usersCommands = usersCommands;
            this.userQueries = userQueries;
            this.generalCommands = generalCommands;
            this.jwtUtils = jwtUtils;
            this.emailService = emailService;
            this.userManager = userManager;
        }

        public StudentsOnPage GetStudentsOnPage(GetCurrentPageRequest request)
        {
            var students = userQueries.GetStudents();

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                students = usersCommands.SearchTextInUsers(request.SearchText.ToLower(), students);
            }

            students = usersCommands.SortByDirection(request.FilterQuery, students);

            var totalCount = students.Count();

            students = generalCommands.GetElementsOnPage(request.FilterQuery, students);

            return new StudentsOnPage
            {
                TotalCount = totalCount,
                Students = usersCommands.FormStudentsList(students)
            };
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

            usersCommands.EditUser(request.User, user);

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

            usersCommands.EdiProfile(request, user);

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