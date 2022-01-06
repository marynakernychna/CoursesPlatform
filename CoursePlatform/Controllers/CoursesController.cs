using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Net;
using CoursesPlatform.Interfaces;
using CoursesPlatform.Models.Courses;
using CoursesPlatform.ErrorMiddleware.Errors;
using CoursesPlatform.Models;
using CoursesPlatform.Models.Users;

namespace CoursesPlatform.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService courseService;
        private readonly IUserAccessor userAccessor;

        public CoursesController(ICourseService courseService,
                                 IUserAccessor userAccessor)
        {
            this.courseService = courseService;
            this.userAccessor = userAccessor;
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("GetCoursesOnPageAdmin")]
        public IActionResult GetCoursesOnPageAdmin(GetCurrentPageRequest request)
        {
            return Ok(courseService.GetCoursesOnAdminPage(request));
        }

        [HttpPost("GetCoursesOnPage")]
        public IActionResult GetCoursesOnPage(FilterQuery request)
        {
            return Ok(courseService.SortAndGetCoursesOnStudentPage(request));
        }

        [HttpPost("GetUserSubscriptionsOnPage")]
        public IActionResult GetUserSubscriptionsOnPage(FilterQuery request)
        {
            return Ok(courseService.SortAndGetUserSubscriptionsOnPage(request));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("AddCourse")]
        public IActionResult AddCourse(AddCourseRequest request)
        {
            courseService.AddCourse(request);

            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("EditCourse")]
        public async Task<IActionResult> EditCourse(CourseDTO request)
        {
            var oldInfo = courseService.GetCourseById(request.Id);

            if (courseService.CheckIsOldUserInfoIsEqualToOld(oldInfo, request))
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "The new information is the same as the previous one !" });
            }

            await courseService.EditCourseAsync(request, oldInfo);

            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("DeleteCourse")]
        public async Task<IActionResult> DeleteCourse(IntRequest request)
        {
            await courseService.DeleteCourseByIdAsync(request.Value);

            return Ok();
        }

        [HttpPost("EnrollInACourse")]
        public IActionResult EnrollInACourse(EnrollCourseRequest request)
        {
            var userId = userAccessor.GetCurrentUserId();

            if (courseService.CheckIsSubscriptionExists(request.CourseId, userId))
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "Subscription already exists !" });
            }

            var newSubscription = courseService
                                  .CreateNewSubscriptionModel(userId, request.CourseId, request.StartDate);

            courseService.AddNewSubscription(newSubscription);

            return Ok();
        }

        [HttpPost("UnsubscribeFromCourse")]
        public IActionResult UnsubscribeFromCourse(UnsubscribeRequest request)
        {
            var userId = userAccessor.GetCurrentUserId();

            if (courseService.CheckIsCourseExistsById(request.CourseId))
            {
                throw new RestException(HttpStatusCode.NotFound, new { Message = "Course not found !" });
            }

            courseService.UnsubscribeFromCourse(userId, request.CourseId);

            return Ok();
        }
    }
}
