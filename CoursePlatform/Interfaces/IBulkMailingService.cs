using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Models.Courses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoursesPlatform.Interfaces
{
    public interface IBulkMailingService
    {
        Task SendCourseRemovalNotificationEmails(List<User> subscribers, string courseTitle);

        Task SendCourseEditingNotificationEmails(List<User> subscribers, CourseDTO newInfo, string oldTitle, string oldDescription);
    }
}
