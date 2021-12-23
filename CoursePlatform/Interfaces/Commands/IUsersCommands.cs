using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Models.Users;
using System.Collections.Generic;
using System.Linq;

namespace CoursesPlatform.Interfaces.Commands
{
    public interface IUsersCommands
    {
        #region form

        List<StudentDTO> FormStudentsList(IQueryable<User> students);

        #endregion

        #region change

        void DeleteUserSubscribes(string userId);

        #endregion
    }
}
