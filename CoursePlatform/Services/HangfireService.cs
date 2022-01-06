using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Interfaces;
using CoursesPlatform.Interfaces.Queries;
using Hangfire;
using System;

namespace CoursesPlatform.Services
{
    public class HangfireService : IHangfireService
    {
        private readonly IEmailService emailService;
        private readonly IHangfireQueries hangfireQueries;

        public HangfireService(IEmailService emailService,
                               IHangfireQueries hangfireQueries)
        {
            this.emailService = emailService;
            this.hangfireQueries = hangfireQueries;
        }

        public void DeleteCourseStartNotifications(int subscriptionId)
        {
            var jobs = hangfireQueries.GetScheduleSubscriptionHangfireJobs(subscriptionId);

            foreach (var item in jobs)
            {
                BackgroundJob.Delete(item.JobId);
            }
        }

        public void SetCourseStartNotifications(UserSubscriptions subscription, string userEmail, string courseTitle)
        {
            var currentDate = DateTime.UtcNow;
            var daysToCourse = subscription.StartDate - currentDate;

            var daysConstants = new DaysConstants(subscription.StartDate);

            if (daysToCourse.Days >= daysConstants.OneDay)
            {

                var job1day = BackgroundJob.Schedule(
                    () => emailService.SendEmailAsync(userEmail, "Start of the course", $"Good day.\n «{courseTitle}» course will start in a day. See you at training."),
                    daysConstants.OneDayDifference);

                hangfireQueries.AddScheduleHangfireJob(new ScheduleHangfireJob
                {
                    JobId = job1day,
                    UserSubscriptionId = subscription.Id
                });

                if (daysToCourse.Days >= daysConstants.SevenDays)
                {
                    var job7days = BackgroundJob.Schedule(
                        () => emailService.SendEmailAsync(userEmail, "Start of the course", $"Good day.\n «{courseTitle}» course will start in a week. See you at training."),
                        daysConstants.SevenDaysDifference);

                    hangfireQueries.AddScheduleHangfireJob(new ScheduleHangfireJob
                    {
                        JobId = job7days,
                        UserSubscriptionId = subscription.Id
                    });
                }
                if (daysToCourse.Days >= daysConstants.ThirtyDays)
                {
                    var job30days = BackgroundJob.Schedule(
                       () => emailService.SendEmailAsync(userEmail, "Start of the course", $"Good day.\n «{courseTitle}» course will start in a month. See you at training."),
                       daysConstants.ThirtyDaysDifference);

                    hangfireQueries.AddScheduleHangfireJob(new ScheduleHangfireJob
                    {
                        JobId = job30days,
                        UserSubscriptionId = subscription.Id
                    });
                }
            }

            hangfireQueries.SaveChanges();
        }
    }
}
