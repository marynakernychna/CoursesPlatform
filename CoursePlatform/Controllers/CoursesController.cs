using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Net;
using CoursesPlatform.Interfaces;
using CoursesPlatform.Models.Courses;
using CoursesPlatform.ErrorMiddleware.Errors;
using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Models;

namespace CoursesPlatform.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService courseService;
        private readonly IUserService userService;
        private readonly IUserAccessor userAccessor;

        public CoursesController(ICourseService courseService,
                                 IUserService userService,
                                 IUserAccessor userAccessor)
        {
            this.courseService = courseService;
            this.userService = userService;
            this.userAccessor = userAccessor;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("GetCourses")]
        public IActionResult GetCourses()
        {
            var courses = courseService.GetCourses();

            return Ok(courses);
        }

        [HttpPost("GetCoursesOnPage")]
        public IActionResult GetCoursesOnPage(FilterQuery request)
        {
            var courses = courseService.SortAndGetCoursesOnPage(request);

            return Ok(courses);
        }

        [HttpPost("GetUserSubscriptionsOnPage")]
        public IActionResult GetUserSubscriptionsOnPage(FilterQuery request)
        {
            var courses = courseService.SortAndGetUserSubscriptionsOnPage(request);

            return Ok(courses);
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
            bool isExists = courseService.CheckIfCourseExistsById(request.Id);

            if (!isExists)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "Course not found !" });
            }

            Course oldInfo = courseService.GetCourseById(request.Id);

            if (oldInfo.Title == request.Title &&
                oldInfo.Description == request.Description &&
                oldInfo.ImageUrl == request.ImageUrl)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "The new information is the same as the previous one !" });
            }

            await courseService.EditCourse(request, oldInfo);

            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("DeleteCourse")]
        public async Task<IActionResult> DeleteCourse(IntRequest request)
        {
            await courseService.DeleteCourseById(request.Value);

            return Ok();
        }

        [HttpPost("EnrollInACourse")]
        public IActionResult EnrollInACourse(EnrollCourseRequest request)
        {
            string userId = userAccessor.GetCurrentUserId();

            bool isExists = courseService.CheckIfCourseExistsById(request.CourseId);

            if (!isExists)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "Course not found !" });
            }

            isExists = courseService.CheckIfSubscriptionExists(request.CourseId, userId);

            if (isExists)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "Subscription already exists !" });
            }

            User user = userService.GetUserById(userId);
            Course course = courseService.GetCourseById(request.CourseId);

            UserSubscriptions newSubscription = courseService
                                                .FormNewSubscription(request.StartDate, user, course);

            courseService.AddNewSubscription(newSubscription);

            return Ok();
        }

        [HttpPost("UnsubscribeFromCourse")]
        public IActionResult UnsubscribeFromCourse(UnsubscribeRequest request)
        {
            string userId = userAccessor.GetCurrentUserId();

            bool isExists = courseService.CheckIfCourseExistsById(request.CourseId);

            if (!isExists)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Message = "Course not found !" });
            }

            courseService.UnsubscribeFromCourse(userId, request.CourseId);

            return Ok();
        }
    }
}
