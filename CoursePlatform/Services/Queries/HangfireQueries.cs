using CoursesPlatform.EntityFramework;
using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Interfaces.Queries;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CoursesPlatform.Services.Queries
{
    public class HangfireQueries : IHangfireQueries
    {
        private readonly AppDbContext appDbContext;

        public HangfireQueries(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public IQueryable<ScheduleHangfireJob> GetScheduleSubscriptionHangfireJobs(int subscriptionId)
        {
            return appDbContext.ScheduleHangfireJobs.AsNoTracking()
                                                    .Where(j => j.UserSubscriptionId == subscriptionId);
        }
    }
}
