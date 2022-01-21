using CoursesPlatform.EntityFramework;
using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.ErrorMiddleware.Errors;
using CoursesPlatform.Interfaces.Queries;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;

namespace CoursesPlatform.Services.Queries
{
    public class UserQueries : IUserQueries
    {
        private readonly AppDbContext appDbContext;

        public UserQueries(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public IQueryable<User> GetStudents()
        {
            var role = appDbContext.Roles.AsNoTracking()
                                         .FirstOrDefault(r => r.Name == StringConstants.StudentRole);

            if (role == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { Message = $"Role <{StringConstants.StudentRole}> not found!" });
            }

            var usersInRoleId = appDbContext.UserRoles.AsNoTracking()
                                                      .Where(u => u.RoleId == role.Id);

            return appDbContext.Users.AsNoTracking()
                                     .Where(u => usersInRoleId.Any(r => r.UserId == u.Id));
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

        public bool CheckIsUserExistsByEmail(string email)
        {
            return appDbContext.Users.FirstOrDefault(u => u.Email == email) != null;
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

        public string GetUserEmailById(string userId)
        {
            return GetUserById(userId).Email;
        }
    }
}
