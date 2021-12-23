using CoursesPlatform.EntityFramework;
using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Interfaces;
using Hangfire;
using System;
using System.Linq;

namespace CoursesPlatform.Services
{
    public class HangfireService : IHangfireService
    {
        private readonly AppDbContext appDbContext;
        private readonly IEmailService emailService;

        public HangfireService(AppDbContext appDbContext,
                               IEmailService emailService)
        {
            this.appDbContext = appDbContext;
            this.emailService = emailService;
        }

        public void DeleteCourseStartNotifications(int subscriptionId)
        {
            var jobs = appDbContext.ScheduleHangfireJobs.Where(j => j.UserSubscriptionId == subscriptionId).ToList();

            foreach (var item in jobs)
            {
                BackgroundJob.Delete(item.JobId);
            }
        }

        public void SetCourseStartNotifications(UserSubscriptions subscription, string userEmail, string courseTitle)
        {
            var currentDate = DateTime.UtcNow;
            TimeSpan daysToCourse = subscription.StartDate - currentDate;

            if (daysToCourse.Days >= 1)
            {
                Days daysConstants = new Days(subscription.StartDate);

                var job1day = BackgroundJob.Schedule(
                    () => emailService.SendEmail(userEmail, "Start of the course", $"Good day.\n «{courseTitle}» course will start in a day. See you at training."),
                    daysConstants.OneDays);

                appDbContext.ScheduleHangfireJobs.Add(new ScheduleHangfireJob
                {
                    JobId = job1day,
                    UserSubscriptionId = subscription.Id
                });

                if (daysToCourse.Days >= 7)
                {
                    var job7days = BackgroundJob.Schedule(
                        () => emailService.SendEmail(userEmail, "Start of the course", $"Good day.\n «{courseTitle}» course will start in a week. See you at training."),
                        daysConstants.SevenDay);

                    appDbContext.ScheduleHangfireJobs.Add(new ScheduleHangfireJob
                    {
                        JobId = job7days,
                        UserSubscriptionId = subscription.Id
                    });
                }
                if (daysToCourse.Days >= 30)
                {
                    var job30days = BackgroundJob.Schedule(
                       () => emailService.SendEmail(userEmail, "Start of the course", $"Good day.\n «{courseTitle}» course will start in a month. See you at training."),
                       daysConstants.ThirtyDays);

                    appDbContext.ScheduleHangfireJobs.Add(new ScheduleHangfireJob
                    {
                        JobId = job30days,
                        UserSubscriptionId = subscription.Id
                    });
                }
            }

            appDbContext.SaveChanges();
        }
    }
}
