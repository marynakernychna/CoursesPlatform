using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Models.Facebook;
using CoursesPlatform.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoursesPlatform.Interfaces
{
    public interface IAuthService
    {
        Task SignInAsync(User user, string password, bool lockoutOnFailure);

        Task RegisterUser(User user, string password);

        User CreateNewUserModel(RegisterRequest request);
		
		Task<FacebookAccount> GetUserFromFacebookAsync(string facebookToken);
    }
}
