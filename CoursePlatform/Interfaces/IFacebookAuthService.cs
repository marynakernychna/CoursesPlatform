using CoursesPlatform.Models.Facebook;
using System.Threading.Tasks;

namespace CoursesPlatform.Interfaces
{
    public interface IFacebookAuthService
    {
        Task<FacebookTokenValidationResult> ValidateAccessTokenAsync(string accessToken);
        Task<FacebookUserInfoResult> GetUserInfoAsync(string accessToken);
    }
}
