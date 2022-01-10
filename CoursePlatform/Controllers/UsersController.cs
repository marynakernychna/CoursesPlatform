using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using CoursesPlatform.Interfaces;
using CoursesPlatform.Models;
using CoursesPlatform.Models.Users;

namespace CoursesPlatform.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IUserAccessor httpUserAccessor;
        private readonly IUtils utils;

        public UsersController(IUserService userService,
                               IUserAccessor httpUserAccessor,
                               IUtils utils)
        {
            this.userService = userService;
            this.httpUserAccessor = httpUserAccessor;
            this.utils = utils;
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("GetStudentsOnPage")]
        public IActionResult GetStudentsOnPage(GetCurrentPageRequest request)
        {
            return Ok(userService.GetStudentsOnPage(request));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("EditUser")]
        public async Task<IActionResult> EditUser(EditUserRequest request)
        {
            await userService.EditUserAsync(request, utils.GetIpAddressOfCurrentRequest(Request, HttpContext), Request);

            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser(StringRequest request)
        {
            await userService.DeleteUserAsync(request, utils.GetIpAddressOfCurrentRequest(Request, HttpContext));

            return Ok();
        }

        [HttpGet("GetProfileInfo")]
        public IActionResult GetProfileInfo()
        {
            return Ok(userService.GetProfileInfo(httpUserAccessor.GetCurrentUserId()));
        }

        [HttpPut("EditProfileInfo")]
        public async Task<IActionResult> EditProfileInfo(EditProfileRequest request)
        {
            await userService.EditProfileInfoAsync(request);

            return Ok();
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            await userService.ChangePasswordAsync(request, httpUserAccessor.GetCurrentUserId());

            return Ok();
        }
    }
}
