using CoursesPlatform.EntityFramework;
using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Interfaces;
using CoursesPlatform.Interfaces.Commands;
using CoursesPlatform.Interfaces.Queries;
using Hangfire;
using System;

namespace CoursesPlatform.Services
{
    public class HangfireService : IHangfireService
    {
        private readonly IEmailService emailService;
        private readonly IHangfireQueries hangfireQueries;
        private readonly IHangfireCommands hangfireCommands;
        private AppDbContext appDbContext;

        public HangfireService(IEmailService emailService,
                               IHangfireQueries hangfireQueries,
                               IHangfireCommands hangfireCommands,
                               AppDbContext appDbContext)
        {
            this.emailService = emailService;
            this.hangfireQueries = hangfireQueries;
            this.hangfireCommands = hangfireCommands;
            this.appDbContext = appDbContext;
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
                    () => emailService.SendCourseStartEmailAsync(courseTitle, daysConstants.Day, userEmail),
                    daysConstants.OneDayDifference);

                hangfireCommands.CreateScheduleHangfireJob(new ScheduleHangfireJob
                {
                    JobId = job1day,
                    UserSubscriptionId = subscription.Id
                });

                if (daysToCourse.Days >= daysConstants.SevenDays)
                {
                    var job7days = BackgroundJob.Schedule(
                        () => emailService.SendCourseStartEmailAsync(courseTitle, daysConstants.Week, userEmail),
                        daysConstants.SevenDaysDifference);

                    hangfireCommands.CreateScheduleHangfireJob(new ScheduleHangfireJob
                    {
                        JobId = job7days,
                        UserSubscriptionId = subscription.Id
                    });
                }
                if (daysToCourse.Days >= daysConstants.ThirtyDays)
                {
                    var job30days = BackgroundJob.Schedule(
                       () => emailService.SendCourseStartEmailAsync(courseTitle, daysConstants.Month, userEmail),
                       daysConstants.ThirtyDaysDifference);

                    hangfireCommands.CreateScheduleHangfireJob(new ScheduleHangfireJob
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
