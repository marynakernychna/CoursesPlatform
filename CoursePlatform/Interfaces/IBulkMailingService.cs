using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Models.Courses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoursesPlatform.Interfaces
{
    public interface IBulkMailingService
    {
        Task SendCourseRemovalNotificationEmailsAsync(List<User> subscribers, string courseTitle);

        Task SendCourseEditingNotificationEmailsAsync(List<User> subscribers, CourseDTO newInfo, string oldTitle, string oldDescription);
    }
}
