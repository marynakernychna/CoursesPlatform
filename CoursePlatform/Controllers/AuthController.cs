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
            User user = userService.GetUserByEmail(request.Email);

            await authService.SignInAsync(user, request.Password, false);

            if (!user.EmailConfirmed)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "Email is not confirmed !" });
            }

            string accessToken = jwtUtils.GenerateAccessToken(user);

            bool isRefreshTokenActive = jwtUtils.CheckIsUserHasActiveRefreshToken(user);

            string refreshToken;

            if (isRefreshTokenActive)
            {
                refreshToken = jwtUtils.GetRefreshToken(user);
            }
            else
            {
                RefreshToken newRefreshToken = jwtUtils.GenerateRefreshToken(IpAddress());

                jwtUtils.SaveRefreshToken(newRefreshToken, user);

                refreshToken = newRefreshToken.Token;
            }

            return Ok(new { accessToken = accessToken, refreshToken = refreshToken });
        }

        [AllowAnonymous]
        [HttpPost("RefreshAccessToken")]
        public IActionResult RefreshAccessToken(TokenRequest request)
        {
            User user = jwtUtils.GetUserByRefreshToken(request.Token);

            bool isRefreshTokenActive = jwtUtils.CheckIsUserHasActiveRefreshToken(user);

            if (!isRefreshTokenActive)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "Invalid refresh token !" });
            }

            var accessToken = jwtUtils.GenerateAccessToken(user);

            return Ok(new { accessToken = accessToken });
        }

        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(RegisterRequest request)
        {
            bool isEmailExists = userService.CheckIfUserExistsByEmail(request.Email);

            if (isEmailExists)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "There is already a user with this email !" });
            }

            User user = authService.FormNewUser(request);

            await authService.RegisterUser(user, request.Password);

            await emailService.SendConfirmationEmail(this.Request, user);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("LogInViaFacebook")]
        public async Task<IActionResult> LogInViaFacebook(StringRequest facebookToken)
        {
            var facebookUser = await authService.GetUserFromFacebookAsync(facebookToken.Value);

            bool isEmailExists = userService.CheckIfUserExistsByEmail(facebookUser.Email);

            if (!isEmailExists)
            {
                const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
                var password = new string(Enumerable.Repeat(chars, 10)
                                                     .Select(s => s[random.Next(s.Length)]).ToArray());

                User newUser = authService.FormNewUser(new RegisterRequest
                {
                    Name = facebookUser.FirstName,
                    Surname = facebookUser.LastName,
                    Email = facebookUser.Email,
                    Birthday = DateTime.Now,
                    Password = password
                });

                await authService.RegisterUser(newUser, password);
            }

            User user = userService.GetUserByEmail(facebookUser.Email);

            string accessToken = jwtUtils.GenerateAccessToken(user);

            bool isRefreshTokenActive = jwtUtils.CheckIsUserHasActiveRefreshToken(user);

            string refreshToken;

            if (isRefreshTokenActive)
            {
                refreshToken = jwtUtils.GetRefreshToken(user);
            }
            else
            {
                RefreshToken newRefreshToken = jwtUtils.GenerateRefreshToken(IpAddress());

                jwtUtils.SaveRefreshToken(newRefreshToken, user);

                refreshToken = newRefreshToken.Token;
            }

            return Ok(new { accessToken = accessToken, refreshToken = refreshToken });
        }

        [AllowAnonymous]
        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmationRequest request)
        {
            bool isUserExists = userService.CheckIfUserExistsByEmail(request.Email);

            if (!isUserExists)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "Incorrect email !" });
            }

            User user = userService.GetUserByEmail(request.Email);

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
