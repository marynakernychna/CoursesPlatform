using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Models.Courses;
using CoursesPlatform.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoursesPlatform.Interfaces
{
    public interface IUserService
    {
        #region get

        //Task<List<StudentDTO>> GetStudents();

        string GetUserIdByEmail(string email);

        string GetUserEmailById(string userId);

        User GetUserById(string id);

        User GetUserByEmail(string email);

        UserDTO GetProfileInfo(string userId);

        StudentsOnPage GetStudentsOnPage(StudentsOnPageRequest request);

        #endregion

        #region change

        void EditUser(UserDTO newInfo, User oldInfo);

        void DeleteUser(User user);

        #endregion

        #region check

        bool CheckIsUserExistsByEmail(string email);

        #endregion

        //Task<StudentsOnPage> SearchByText(SearchStudentsRequest request);

        void EdiProfile(EditProfileRequest newInfo, User currentInfo);

    }
}