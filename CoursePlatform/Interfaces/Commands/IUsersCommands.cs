using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Models.Courses;
using CoursesPlatform.Models.Facebook;
using CoursesPlatform.Models.Users;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesPlatform.Interfaces.Commands
{
    public interface IUsersCommands
    {
        Task SignInAsync(User user, string password, bool lockoutOnFailure);

        User CreateNewUserModel(RegisterRequest request);

        Task<FacebookAccount> GetUserFromFacebookAsync(string facebookToken);

        IQueryable<User> SearchTextInUsers(string searchText, IQueryable<User> users);

        IQueryable<User> SortByDirection(FilterQuery filterQuery, IQueryable<User> students);

        List<StudentDTO> FormStudentsList(IQueryable<User> students);

        void EditUser(UserDTO newInfo, User user);

        void DeleteUser(User user);

        void EdiProfile(EditProfileRequest newInfo, User user);
    }
}
