using CoursesPlatform.EntityFramework;
using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Interfaces.Queries;
using System.Linq;

namespace CoursesPlatform.Services.Queries
{
    public class HangfireQueries : IHangfireQueries
    {
        private AppDbContext appDbContext;

        public HangfireQueries(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public IQueryable<ScheduleHangfireJob> GetScheduleSubscriptionHangfireJobs(int subscriptionId)
        {
            return appDbContext.ScheduleHangfireJobs.Where(j => j.UserSubscriptionId == subscriptionId);
        }

        public void AddScheduleHangfireJob(ScheduleHangfireJob scheduleHangfireJob)
        {
            appDbContext.ScheduleHangfireJobs.Add(scheduleHangfireJob);
        }

        public void SaveChanges()
        {
            appDbContext.SaveChanges();
        }
    }
}
