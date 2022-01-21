using CoursesPlatform.EntityFramework;
using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.Interfaces.Commands;

namespace CoursesPlatform.Services.Commands
{
    public class HangfireCommands : IHangfireCommands
    {
        private AppDbContext appDbContext;

        public HangfireCommands(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public void CreateScheduleHangfireJob(ScheduleHangfireJob scheduleHangfireJob)
        {
            appDbContext.ScheduleHangfireJobs.Add(scheduleHangfireJob);
        }
    }
}
