using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Interfaces;
using CoursesPlatform.Models.Courses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoursesPlatform.Services
{
    public class BulkMailingService : IBulkMailingService
    {
        private readonly IEmailService emailService;

        public BulkMailingService(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public async Task SendCourseRemovalNotificationEmails(List<User> subscribers, string courseTitle)
        {
            foreach (var user in subscribers)
            {
                await emailService.SendCourseRemovalNotificationEmail(courseTitle, user);
            }
        }

        public async Task SendCourseEditingNotificationEmails(List<User> subscribers, CourseDTO newInfo, string oldTitle, string oldDescription)
        {
            foreach (var user in subscribers)
            {
                await emailService.SendCourseEditingNotificationEmail(newInfo, oldTitle, oldDescription, user);
            }
        }
    }
}
