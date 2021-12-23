using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Net;
using CoursesPlatform.Interfaces;
using CoursesPlatform.Models;
using CoursesPlatform.ErrorMiddleware.Errors;
using CoursesPlatform.Models.Users;
using CoursesPlatform.EntityFramework.Models;

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

        public UsersController(IUserService userService,
                               IUserAccessor httpUserAccessor,
                               IEmailService emailService,
                               IJwtUtils jwtUtils)
        {
            this.userService = userService;
            this.httpUserAccessor = httpUserAccessor;
            this.emailService = emailService;
            this.jwtUtils = jwtUtils;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("GetStudents")]
        public async Task<IActionResult> GetStudents()
        {
            var students = await userService.GetStudents();

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

            await emailService.SendEmailChangingNotificationEmail(user, oldInfo);

            await emailService.SendConfirmationEmail(this.Request, user);

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

