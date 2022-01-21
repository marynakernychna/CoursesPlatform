using CoursesPlatform.EntityFramework;
using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.ErrorMiddleware.Errors;
using CoursesPlatform.Interfaces.Commands;
using CoursesPlatform.Interfaces.Queries;
using CoursesPlatform.Models.Users;
using System;
using System.Net;

namespace CoursesPlatform.Services.Commands
{
    public class UsersCommands : IUsersCommands
    {
        private readonly ICoursesCommands coursesCommands;
        private readonly ICoursesQueries coursesQueries;
        private readonly IUserQueries userQueries;
        private AppDbContext appDbContext;

        public UsersCommands(ICoursesCommands coursesCommands,
                             IUserQueries userQueries,
                             ICoursesQueries coursesQueries,
                             AppDbContext appDbContext)
        {
            this.coursesCommands = coursesCommands;
            this.userQueries = userQueries;
            this.coursesQueries = coursesQueries;
            this.appDbContext = appDbContext;
        }

        public void UpdateUser(UserDTO newInfo, User user)
        {
            User oldUser = userQueries.GetUserByEmail(user.Email);

            if (newInfo.Email != user.Email)
            {
                var isEmailBusy = userQueries.CheckIsUserExistsByEmail(newInfo.Email);

                if (isEmailBusy)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Message = "There is already a user with this email !" });
                }

                oldUser.Email = newInfo.Email;
                oldUser.UserName = newInfo.Email;
                oldUser.NormalizedEmail = newInfo.Email.ToUpper();
                oldUser.NormalizedUserName = newInfo.Email.ToUpper();
                oldUser.EmailConfirmed = false;
            }

            oldUser.Name = newInfo.Name;
            oldUser.Surname = newInfo.Surname;
            oldUser.Birthday = newInfo.Birthday;

            appDbContext.Users.Update(oldUser);
            appDbContext.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            DeleteUserSubscribes(user.Id);

            appDbContext.Users.Remove(user);
            appDbContext.SaveChanges();
        }

        private void DeleteUserSubscribes(string userId)
        {
            var subscriptions = coursesQueries.GetUserSubscriptions(userId);

            coursesCommands.DeleteUserSubscriptions(subscriptions);
        }

        public void UpdateProfile(EditProfileRequest newInfo, User user)
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

            appDbContext.SaveChanges();
        }
    }
}
