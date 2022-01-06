using CoursesPlatform.EntityFramework;
using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.ErrorMiddleware.Errors;
using CoursesPlatform.Interfaces.Queries;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CoursesPlatform.Services.Queries
{
    public class UserQueries : IUserQueries
    {
        private AppDbContext appDbContext;
        private UserManager<User> userManager;

        public UserQueries(AppDbContext appDbContext,
                           UserManager<User> userManager)
        {
            this.appDbContext = appDbContext;
            this.userManager = userManager;
        }

        public User GetUserByEmail(string email)
        {
            var user = appDbContext.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { Message = "User not found by email !" });
            }

            return user;
        }

        public User GetUserById(string id)
        {
            var user = appDbContext.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { Message = "User not found !" });
            }

            return user;
        }

        public bool CheckIsUserExistsByEmail(string email)
        {
            return appDbContext.Users.FirstOrDefault(u => u.Email == email) != null;
        }

        public async Task RegisterStudentAsync(User user, string password)
        {
            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new RestException(HttpStatusCode.InternalServerError, new { Message = "Failed to create user!" });
            }

            result = await userManager.AddToRoleAsync(user, StringConstants.StudentRole);

            if (!result.Succeeded)
            {
                throw new RestException(HttpStatusCode.InternalServerError, new { Message = "Failed to add user to role!" });
            }

            appDbContext.SaveChanges();
        }

        public string GetUserEmailById(string userId)
        {
            return GetUserById(userId).Email;
        }

        public IQueryable<User> GetStudents()
        {
            var role = appDbContext.Roles.FirstOrDefault(r => r.Name == "Student");

            if (role == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { Message = "Role <Student> not found!" });
            }

            var usersInRoleId = appDbContext.UserRoles.Where(u => u.RoleId == role.Id);

            return appDbContext.Users.Where(u => usersInRoleId.Any(r => r.UserId == u.Id));
        }

        public void SaveChanges()
        {
            appDbContext.SaveChanges();
        }

        public void RemoveUser(User user)
        {
            appDbContext.Users.Remove(user);

            appDbContext.SaveChanges();
        }
    }
}
