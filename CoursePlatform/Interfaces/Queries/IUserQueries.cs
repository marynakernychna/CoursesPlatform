using CoursesPlatform.EntityFramework.Models;
using System.Linq;

namespace CoursesPlatform.Interfaces.Queries
{
    public interface IUserQueries
    {
        User GetUserByEmail(string email);

        User GetUserById(string id);

        bool CheckIsUserExistsByEmail(string email);

        string GetUserEmailById(string userId);

        IQueryable<User> GetStudents();
    }
}
