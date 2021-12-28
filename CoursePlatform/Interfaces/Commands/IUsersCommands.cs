using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Models.Courses;
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

        List<StudentDTO> GetStudentsOnPage(FilterQuery request, IQueryable<User> students);
    }
}
