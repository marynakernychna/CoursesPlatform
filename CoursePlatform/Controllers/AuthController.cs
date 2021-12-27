using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CoursesPlatform.Interfaces;
using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.ErrorMiddleware.Errors;
using System.Net;
using CoursesPlatform.Models.Users;
using CoursesPlatform.Models;
using System;
using System.Linq;

namespace CoursesPlatform.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly IUserService userService;
        private readonly IJwtUtils jwtUtils;
        private readonly IEmailService emailService;
        private readonly UserManager<User> userManager;
        private static Random random = new Random();

        public AuthController(IAuthService authService,
                              IUserService userService,
                              IJwtUtils jwtUtils,
                              IEmailService emailService,
                              UserManager<User> userManager)
        {
            this.authService = authService;
            this.userService = userService;
            this.emailService = emailService;
            this.jwtUtils = jwtUtils;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn(AuthenticateRequest request)
        {
            var user = userService.GetUserByEmail(request.Email);

            await authService.SignInAsync(user, request.Password, false);

            if (!user.EmailConfirmed)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "Email is not confirmed!" });
            }

            var accessToken = jwtUtils.GenerateAccessToken(user);

            var isRefreshTokenActive = jwtUtils.CheckIsUserHasActiveRefreshToken(user);

            string refreshToken = null;

            if (isRefreshTokenActive)
            {
                refreshToken = jwtUtils.GetRefreshToken(user);
            }
            else
            {
                var newRefreshToken = jwtUtils.GenerateRefreshToken(IpAddress());

                jwtUtils.SaveRefreshToken(newRefreshToken, user);

                refreshToken = newRefreshToken.Token;
            }

            return Ok(new { accessToken = accessToken, refreshToken = refreshToken });
        }

        [AllowAnonymous]
        [HttpPost("RefreshAccessToken")]
        public IActionResult RefreshAccessToken(TokenRequest request)
        {
            var user = jwtUtils.GetUserByRefreshToken(request.Token);

            var isRefreshTokenActive = jwtUtils.CheckIsUserHasActiveRefreshToken(user);

            if (!isRefreshTokenActive)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "Invalid refresh token!" });
            }

            var accessToken = jwtUtils.GenerateAccessToken(user);

            return Ok(new { accessToken = accessToken });
        }

        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(RegisterRequest request)
        {
            var isEmailExists = userService.CheckIsUserExistsByEmail(request.Email);

            if (isEmailExists)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "There is already a user with this email!" });
            }

            var user = authService.CreateNewUserModel(request);

            await authService.RegisterUser(user, request.Password);

            await emailService.SendConfirmationEmail(Request, user);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("LogInViaFacebook")]
        public async Task<IActionResult> LogInViaFacebook(StringRequest facebookToken)
        {
            var facebookUser = await authService.GetUserFromFacebookAsync(facebookToken.Value);

            var isEmailExists = userService.CheckIsUserExistsByEmail(facebookUser.Email);

            if (!isEmailExists)
            {
                const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
                var password = new string(Enumerable.Repeat(chars, 10)
                                                     .Select(s => s[random.Next(s.Length)]).ToArray());

                User newUser = authService.CreateNewUserModel(new RegisterRequest
                {
                    Name = facebookUser.FirstName,
                    Surname = facebookUser.LastName,
                    Email = facebookUser.Email,
                    Birthday = DateTime.UtcNow,
                    Password = password
                });

                await authService.RegisterUser(newUser, password);
            }

            var user = userService.GetUserByEmail(facebookUser.Email);

            var accessToken = jwtUtils.GenerateAccessToken(user);

            var isRefreshTokenActive = jwtUtils.CheckIsUserHasActiveRefreshToken(user);

            string refreshToken = null;

            if (isRefreshTokenActive)
            {
                refreshToken = jwtUtils.GetRefreshToken(user);
            }
            else
            {
                var newRefreshToken = jwtUtils.GenerateRefreshToken(IpAddress());

                jwtUtils.SaveRefreshToken(newRefreshToken, user);

                var = newRefreshToken.Token;
            }

            return Ok(new { accessToken = accessToken, refreshToken = refreshToken });
        }

        [AllowAnonymous]
        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmationRequest request)
        {
            var isUserExists = userService.CheckIsUserExistsByEmail(request.Email);

            if (!isUserExists)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "Incorrect email !" });
            }

            var user = userService.GetUserByEmail(request.Email);

            await userManager.ConfirmEmailAsync(user, request.Token);

            return Ok();
        }
		
		public string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"];
            }
            else
            {
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
        }
    }
}
