using CoursesPlatform.Models;
using CoursesPlatform.Models.Users;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CoursesPlatform.Interfaces
{
    public interface IAuthService
    {
        Task<AuthenticateResponse> LogInAsync(AuthenticateRequest request, string ipAddress);

        Task RegisterUserAsync(RegisterRequest request, HttpRequest httpRequest);

        Task<AuthenticateResponse> LogInViaFacebookAsync(StringRequest facebookToken, string ipAddress);

        Task ConfirmEmailAsync(EmailConfirmationRequest request);

        Task<string> RefreshAccessTokenAsync(TokenRequest request);
    }
}
