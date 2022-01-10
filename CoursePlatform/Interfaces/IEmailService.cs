using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Models.Courses;
using CoursesPlatform.Models.Users;
using System.Threading.Tasks;

namespace CoursesPlatform.Interfaces
{
    public interface IEmailService
    {
        Task SendCourseStartEmailAsync(string courseTitle, string startIn, string userEmail);

        Task SendConfirmationEmailAsync(User user);

        Task SendCourseRemovalNotificationEmailAsync(string courseTitle, User user);

        Task SendCourseEditingNotificationEmailAsync(CourseDTO newInfo, string oldTitle, string oldDescription, User user);

        Task SendEmailAsync(string email, string subject, string message);

        Task SendUserInfoChangingNotificationEmailAsync(User newInfo, UserDTO oldInfo);

        Task SendAccountRemovalNotificationEmailAsync(UserDTO user);
    }
}
