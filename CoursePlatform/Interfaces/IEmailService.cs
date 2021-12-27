using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Models.Courses;
using CoursesPlatform.Models.Users;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CoursesPlatform.Interfaces
{
    public interface IEmailService
    {
        Task SendConfirmationEmail(HttpRequest request, User user);

        Task SendCourseRemovalNotificationEmail(string courseTitle, User user);

        Task SendCourseEditingNotificationEmail(CourseDTO newInfo, string oldTitle, string oldDescription, User user);

        Task SendEmail(string email, string subject, string message);

        Task SendUserInfoChangingNotificationEmail(User newInfo, UserDTO oldInfo);

        Task SendAccountRemovalNotificationEmail(UserDTO user);
    }
}
