using CoursesPlatform.EntityFramework.Models;
using System.Linq;

namespace CoursesPlatform.Interfaces.Queries
{
    public interface IHangfireQueries
    {
        IQueryable<ScheduleHangfireJob> GetScheduleSubscriptionHangfireJobs(int subscriptionId);
    }
}
