using CoursesPlatform.Models;
using CoursesPlatform.Models.Users;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CoursesPlatform.Interfaces
{
    public interface IUserService
    {
        StudentsOnPage GetStudentsOnPage(GetCurrentPageRequest request);

        Task EditUserAsync(EditUserRequest request, string ipAddress, HttpRequest httpRequest);

        Task DeleteUserAsync(StringRequest request, string ipAddress);

        UserDTO GetProfileInfo(string userId);

        Task EditProfileInfoAsync(EditProfileRequest request);

        Task ChangePasswordAsync(ChangePasswordRequest request, string userId);
    }
}