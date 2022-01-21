using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Models.Users;

namespace CoursesPlatform.Interfaces.Commands
{
    public interface IUsersCommands
    {
        void UpdateUser(UserDTO newInfo, User user);

        void DeleteUser(User user);

        void UpdateProfile(EditProfileRequest newInfo, User user);
    }
}
