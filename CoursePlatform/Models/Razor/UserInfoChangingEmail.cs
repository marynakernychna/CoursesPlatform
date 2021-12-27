using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Models.Users;

namespace CoursesPlatform.Models.Razor
{
    public class UserInfoChangingEmail
    {
        public User NewInfo { get; set; }

        public UserDTO OldInfo { get; set; }

        public bool IsNameChanged => NewInfo.Name != OldInfo.Name;

        public bool IsSurnameChanged => NewInfo.Surname != OldInfo.Surname;

        public bool IsEmailChanged => NewInfo.Email != OldInfo.Email;
    }
}
