using CoursesPlatform.EntityFramework.Models;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesPlatform.Interfaces.Queries
{
    public interface IUserQueries
    {
        User GetUserByEmail(string email);

        User GetUserById(string id);

        bool CheckIsUserExistsByEmail(string email);

        Task RegisterStudentAsync(User user, string password);

        string GetUserEmailById(string userId);

        IQueryable<User> GetStudents();

        void SaveChanges();

        void RemoveUser(User user);
    }
}
