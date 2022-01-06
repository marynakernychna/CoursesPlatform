using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using CoursesPlatform.Interfaces;
using CoursesPlatform.Models.Users;
using CoursesPlatform.Models;

namespace CoursesPlatform.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly IUtils utils;

        public AuthController(IAuthService authService,
                              IUtils utils)
        {
            this.authService = authService;
            this.utils = utils;
        }

        [AllowAnonymous]
        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn(AuthenticateRequest request)
        {
            return Ok(await authService.LogInAsync(request, utils.GetIpAddressOfCurrentRequest(Request, HttpContext)));
        }

        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(RegisterRequest request)
        {
            await authService.RegisterUserAsync(request, Request);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("LogInViaFacebook")]
        public async Task<IActionResult> LogInViaFacebook(StringRequest facebookToken)
        {
            return Ok(await authService.LogInViaFacebookAsync(facebookToken, utils.GetIpAddressOfCurrentRequest(Request, HttpContext)));
        }

        [AllowAnonymous]
        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmationRequest request)
        {
            await authService.ConfirmEmailAsync(request);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("RefreshAccessToken")]
        public async Task<IActionResult> RefreshAccessToken(TokenRequest request)
        {
            return Ok(new { accessToken = await authService.RefreshAccessTokenAsync(request) });
        }
    }
}
