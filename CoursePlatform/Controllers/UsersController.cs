using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Net;
using CoursesPlatform.Interfaces;
using CoursesPlatform.Models;
using CoursesPlatform.ErrorMiddleware.Errors;
using CoursesPlatform.Models.Users;
using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Models.Courses;
using System;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.AspNetCore.Identity;

namespace CoursesPlatform.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IUserAccessor httpUserAccessor;
        private readonly IEmailService emailService;
        private readonly IJwtUtils jwtUtils;
        private readonly UserManager<User> userManager;

        public UsersController(IUserService userService,
                               IUserAccessor httpUserAccessor,
                               IEmailService emailService,
                               UserManager<User> userManager,
                               IJwtUtils jwtUtils)
        {
            this.userService = userService;
            this.httpUserAccessor = httpUserAccessor;
            this.emailService = emailService;
            this.userManager = userManager;
            this.jwtUtils = jwtUtils;
        }

        [Authorize(Roles = "Administrator")]
        [AllowAnonymous]
        [HttpPost("GetStudentsOnPage")]
        public IActionResult GetStudentsOnPage(OnPageRequest request)
        {
            var students = userService.GetStudentsOnPage(request);

            return Ok(students);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("EditUser")]
        public async Task<IActionResult> EditUser(EditUserRequest request)
        {
            var user = userService.GetUserByEmail(request.CurrentUserEmail);

            if (user.Name == request.User.Name &&
                user.Surname == request.User.Surname &&
                user.Email == request.User.Email)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "The new information is the same as the previous one !" });
            }

            var oldInfo = new UserDTO
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Birthday = user.Birthday
            };

            jwtUtils.RevokeAccess(user, IpAddress());

            userService.EditUser(request.User, user);

            await emailService.SendUserInfoChangingNotificationEmail(user, oldInfo);

            if (oldInfo.Email != user.Email)
            {
                await emailService.SendConfirmationEmail(this.Request, user);
            }

            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("DeleteUser")]
        public IActionResult DeleteUser(StringRequest request)
        {
            User user = userService.GetUserByEmail(request.Value);

            var userDTO = new UserDTO
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Birthday = System.DateTime.Now
            };

            jwtUtils.RevokeAccess(user, IpAddress());

            userService.DeleteUser(user);

            emailService.SendAccountRemovalNotificationEmail(userDTO);

            return Ok();
        }

        [HttpGet("GetProfileInfo")]
        public IActionResult GetProfileInfo()
        {
            string userId = httpUserAccessor.GetCurrentUserId();

            var profileInfo = userService.GetProfileInfo(userId);

            return Ok(profileInfo);
        }

        //[Authorize(Roles = "Administrator")]
        //[HttpPost("SearchText")]
        //public async Task<IActionResult> SearchText(SearchStudentsRequest request)
        //{
        //    var result = await userService.SearchByText(request);

        //    return Ok(result);
        //}

        [HttpPost("EditProfileInfo")]
        public async Task<IActionResult> EditProfileInfo(EditProfileRequest request)
        {
            var user = userService.GetUserByEmail(request.CurrentEmail);

            if (user.Name == request.Name &&
                user.Surname == request.Surname &&
                user.Email == request.Email &&
                (string.IsNullOrEmpty(request.Birthday) ||
                string.IsNullOrWhiteSpace(request.Birthday) || 
                Convert.ToDateTime(request.Birthday) == user.Birthday))
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "The new information is the same as the previous one !" });
            }

            var oldInfo = new UserDTO
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Birthday = user.Birthday
            };

            userService.EdiProfile(request, user);

            await emailService.SendUserInfoChangingNotificationEmail(user, oldInfo);

            if (oldInfo.Email != user.Email)
            {
                await emailService.SendConfirmationEmail(this.Request, user);
            }
            return Ok();
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var userId = httpUserAccessor.GetCurrentUserId();

            var user = userService.GetUserById(userId);

            var result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

            if (!result.Succeeded)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "Failed to change password!" });
            }

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

